using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Spreadsheets
{
    public interface IEntityMapper<T> where T: class
    {
        /// <summary>
        /// Creates an instance of the entity using the EntityType
        /// </summary>
        /// <returns></returns>
        object CreateEntity();


        /// <summary>
        /// Registers the sequence of fields ( either from a row or column ) that should be mapped using labels supplied.
        /// The mapping of the fields occurs in a linear approach by mapping the labels to the fields.
        /// </summary>
        /// <param name="fieldNames"></param>
        void RegisterFieldSequence(List<string> fieldNames);


        /// <summary>
        /// Adds a field that should be persisted.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isRequired"></param>
        /// <param name="defaultVal"></param>
        EntityField AddField<TField>(string name, bool isRequired, string defaultVal);


        /// <summary>
        /// Adds a field that should be persisted.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="isRequired"></param>
        /// <param name="defaultVal"></param>
        /// <param name="minLen"></param>
        /// <param name="maxLen"></param>
        EntityField AddField(string label, string name, Type type, bool isRequired, string defaultVal, int minLen, int maxLen);


        /// <summary>
        /// Adds a field that should be persisted.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="isRequired"></param>
        /// <param name="defaultVal"></param>
        /// <param name="minLen"></param>
        /// <param name="maxLen"></param>
        EntityField AddField(string label, string name, string typeName, Type type, bool isRequired, string defaultVal, int minLen, int maxLen);
        

        /// <summary>
        /// Maps an entity start from the row / col supplied. The number of fields to map is reprsented by the Fields that have been registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startRow">The start row containing the first entity to map</param>
        /// <param name="startCol">The start column containing the first entity to map</param>
        /// <param name="isVertical"></param>
        /// <returns></returns>
        T MapEntity(int startRow, int startCol, bool isVertical);
        
        
        /// <summary>
        /// Maps entities either across or down.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">The name of the sheet containing the entities</param>
        /// <param name="startRow">The start row to map from</param>
        /// <param name="startCol">The start column to map from</param>
        /// <param name="numCols">The number of columns to map</param>
        /// <param name="numRows">The number of rows to map</param>
        /// <param name="isVerticalEntity">Whether or not this mapping is done vertically.</param>
        /// <returns></returns>
        List<T> MapEntities(string sheet, int startRow, int startCol, int numCols, int numRows, bool isVerticalEntity);
        
        
        /// <summary>
        /// Maps the entities where each entity exists in its own COLUMN.
        /// </summary>
        /// <returns></returns>
        List<T> MapEntitiesDown(string sheet, int startRow, int startCol, int numCols, int numRows);


        /// <summary>
        /// Maps the entities where each entity exists in its own ROW
        /// </summary>
        /// <returns></returns>
        List<T> MapEntitiesAcross(string sheet, int startRow, int startCol, int numCols, int numRows);


        /// <summary>
        /// Maps the metadata in the sheet so that the mappings themselves can by made dynamic by putting 
        /// getting them from the spreadsheet rather than hardcoding.
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// </summary>
        void MapMetadata(string sheet, int startRow, int startCol);
    }
}
