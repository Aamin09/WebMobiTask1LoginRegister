using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1LoginRegister.Migrations
{
    public partial class CorrectedVariantAttributeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Check if we need to rename the column
                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('VariantAttributeValues') AND name = 'VarinatId')
                BEGIN
                    -- First drop the foreign key constraint if it exists
                    DECLARE @fkName NVARCHAR(128);
                    SELECT @fkName = name FROM sys.foreign_keys 
                    WHERE parent_object_id = OBJECT_ID('VariantAttributeValues')
                    AND referenced_object_id = OBJECT_ID('ProductVariants');
                    
                    IF @fkName IS NOT NULL
                    BEGIN
                        EXEC('ALTER TABLE VariantAttributeValues DROP CONSTRAINT ' + @fkName);
                    END
                    
                    -- Drop the index if it exists
                    DECLARE @indexName NVARCHAR(128);
                    SELECT @indexName = name FROM sys.indexes 
                    WHERE object_id = OBJECT_ID('VariantAttributeValues') 
                    AND name = 'IX_VariantAttributeValues_VarinatId';
                    
                    IF @indexName IS NOT NULL
                    BEGIN
                        EXEC('DROP INDEX IX_VariantAttributeValues_VarinatId ON VariantAttributeValues');
                    END
                    
                    -- Check if VariantId already exists
                    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('VariantAttributeValues') AND name = 'VariantId')
                    BEGIN
                        -- Rename the column
                        EXEC sp_rename 'VariantAttributeValues.VarinatId', 'VariantId', 'COLUMN';
                    END
                    ELSE
                    BEGIN
                        -- Both columns exist - copy data and drop old column
                        UPDATE VariantAttributeValues SET VariantId = VarinatId WHERE VariantId IS NULL OR VariantId = 0;
                        ALTER TABLE VariantAttributeValues DROP COLUMN VarinatId;
                    END
                END");

            // Recreate the index and foreign key with correct names
            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeValues_VariantId",
                table: "VariantAttributeValues",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_VariantAttributeValues_ProductVariants_VariantId",
                table: "VariantAttributeValues",
                column: "VariantId",
                principalTable: "ProductVariants",
                principalColumn: "VariantId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariantAttributeValues_ProductVariants_VariantId",
                table: "VariantAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_VariantAttributeValues_VariantId",
                table: "VariantAttributeValues");

            migrationBuilder.Sql(@"
                -- Revert to original column name if needed
                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('VariantAttributeValues') AND name = 'VariantId')
                AND NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('VariantAttributeValues') AND name = 'VarinatId')
                BEGIN
                    -- First ensure the column is nullable for rollback
                    ALTER TABLE VariantAttributeValues ALTER COLUMN VariantId INT NULL;
                    
                    -- Add old column if it doesn't exist
                    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('VariantAttributeValues') AND name = 'VarinatId')
                    BEGIN
                        ALTER TABLE VariantAttributeValues ADD VarinatId INT NULL;
                    END
                    
                    -- Copy data back
                    UPDATE VariantAttributeValues SET VarinatId = VariantId;
                    
                    -- Make old column non-nullable
                    ALTER TABLE VariantAttributeValues ALTER COLUMN VarinatId INT NOT NULL;
                    
                    -- Drop the new column
                    ALTER TABLE VariantAttributeValues DROP COLUMN VariantId;
                    
                    -- Rename back to original
                    EXEC sp_rename 'VariantAttributeValues.VarinatId', 'VariantId', 'COLUMN';
                END");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttributeValues_VarinatId",
                table: "VariantAttributeValues",
                column: "VarinatId");

            migrationBuilder.AddForeignKey(
                name: "FK_VariantAttributeValues_ProductVariants_VarinatId",
                table: "VariantAttributeValues",
                column: "VarinatId",
                principalTable: "ProductVariants",
                principalColumn: "VariantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}