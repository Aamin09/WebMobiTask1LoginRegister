using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1LoginRegister.Migrations
{
    /// <inheritdoc />
    public partial class AddedMainProductAttrbuteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Check if we need to rename the column (only if old name exists and new name doesn't)
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AttrbuteValueId' AND Object_ID = Object_ID(N'VariantAttributeValues'))
                AND NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AttributeValueId' AND Object_ID = Object_ID(N'VariantAttributeValues'))
                BEGIN
                    EXEC sp_rename 'VariantAttributeValues.AttrbuteValueId', 'AttributeValueId', 'COLUMN';
                END
            ");

            // Step 2: Ensure the column exists with the correct name (create if needed)
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AttributeValueId' AND Object_ID = Object_ID(N'VariantAttributeValues'))
                BEGIN
                    ALTER TABLE [VariantAttributeValues] ADD [AttributeValueId] INT NULL;
                    
                    -- Copy data from old column if it exists
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AttrbuteValueId' AND Object_ID = Object_ID(N'VariantAttributeValues'))
                    BEGIN
                        UPDATE [VariantAttributeValues] SET [AttributeValueId] = [AttrbuteValueId];
                    END
                    
                    -- Make the column non-nullable if needed
                    ALTER TABLE [VariantAttributeValues] ALTER COLUMN [AttributeValueId] INT NOT NULL;
                END
            ");

            // Step 3: Recreate the index with the new name if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_VariantAttributeValues_AttributeValueId' AND object_id = OBJECT_ID('VariantAttributeValues'))
                BEGIN
                    -- Drop old index if it exists
                    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_VariantAttributeValues_AttrbuteValueId' AND object_id = OBJECT_ID('VariantAttributeValues'))
                    BEGIN
                        DROP INDEX [IX_VariantAttributeValues_AttrbuteValueId] ON [VariantAttributeValues];
                    END
                    
                    CREATE INDEX [IX_VariantAttributeValues_AttributeValueId] ON [VariantAttributeValues] ([AttributeValueId]);
                END
            ");

            // Step 4: Recreate the foreign key with the new name if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_VariantAttributeValues_ProductAttributeValues_AttributeValueId')
                BEGIN
                    -- Drop old foreign key if it exists
                    DECLARE @constraintName NVARCHAR(128)
                    SELECT @constraintName = name FROM sys.foreign_keys 
                    WHERE parent_object_id = OBJECT_ID('VariantAttributeValues')
                    AND referenced_object_id = OBJECT_ID('ProductAttributeValues')
                    AND name LIKE 'FK_VariantAttributeValues_ProductAttributeValues_%'
                    
                    IF @constraintName IS NOT NULL
                    BEGIN
                        EXEC('ALTER TABLE [VariantAttributeValues] DROP CONSTRAINT [' + @constraintName + ']');
                    END
                    
                    ALTER TABLE [VariantAttributeValues] WITH CHECK ADD 
                    CONSTRAINT [FK_VariantAttributeValues_ProductAttributeValues_AttributeValueId] 
                    FOREIGN KEY ([AttributeValueId]) REFERENCES [ProductAttributeValues] ([ValueId]) ON DELETE NO ACTION;
                END
            ");

            // Step 5: Drop the old column if it exists and we don't need it anymore
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AttrbuteValueId' AND Object_ID = Object_ID(N'VariantAttributeValues'))
                AND EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AttributeValueId' AND Object_ID = Object_ID(N'VariantAttributeValues'))
                BEGIN
                    ALTER TABLE [VariantAttributeValues] DROP COLUMN [AttrbuteValueId];
                END
            ");

            // Step 6: Create the new ProductAttributeValueMappings table
            migrationBuilder.CreateTable(
                name: "ProductAttributeValueMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    AttributeValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValueMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValueMappings_ProductAttributeValues_AttributeValueId",
                        column: x => x.AttributeValueId,
                        principalTable: "ProductAttributeValues",
                        principalColumn: "ValueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValueMappings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValueMappings_AttributeValueId",
                table: "ProductAttributeValueMappings",
                column: "AttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValueMappings_ProductId",
                table: "ProductAttributeValueMappings",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop the new table
            migrationBuilder.DropTable(
                name: "ProductAttributeValueMappings");

            // Step 2: Restore the original column if needed
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AttrbuteValueId' AND Object_ID = Object_ID(N'VariantAttributeValues'))
                AND EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AttributeValueId' AND Object_ID = Object_ID(N'VariantAttributeValues'))
                BEGIN
                    -- Add the old column back
                    ALTER TABLE [VariantAttributeValues] ADD [AttrbuteValueId] INT NULL;
                    
                    -- Copy data back
                    UPDATE [VariantAttributeValues] SET [AttrbuteValueId] = [AttributeValueId];
                    
                    -- Make it non-nullable
                    ALTER TABLE [VariantAttributeValues] ALTER COLUMN [AttrbuteValueId] INT NOT NULL;
                    
                    -- Recreate index
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_VariantAttributeValues_AttrbuteValueId' AND object_id = OBJECT_ID('VariantAttributeValues'))
                    BEGIN
                        CREATE INDEX [IX_VariantAttributeValues_AttrbuteValueId] ON [VariantAttributeValues] ([AttrbuteValueId]);
                    END
                    
                    -- Recreate foreign key
                    DECLARE @constraintName NVARCHAR(128)
                    SELECT @constraintName = name FROM sys.foreign_keys 
                    WHERE parent_object_id = OBJECT_ID('VariantAttributeValues')
                    AND referenced_object_id = OBJECT_ID('ProductAttributeValues')
                    AND name LIKE 'FK_VariantAttributeValues_ProductAttributeValues_%'
                    
                    IF @constraintName IS NOT NULL
                    BEGIN
                        EXEC('ALTER TABLE [VariantAttributeValues] DROP CONSTRAINT [' + @constraintName + ']');
                    END
                    
                    ALTER TABLE [VariantAttributeValues] WITH CHECK ADD 
                    CONSTRAINT [FK_VariantAttributeValues_ProductAttributeValues_AttrbuteValueId] 
                    FOREIGN KEY ([AttrbuteValueId]) REFERENCES [ProductAttributeValues] ([ValueId]) ON DELETE NO ACTION;
                    
                    -- Drop the new column
                    ALTER TABLE [VariantAttributeValues] DROP COLUMN [AttributeValueId];
                END
            ");
        }
    }
}