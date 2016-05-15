# Overview
The CMS component extracts data from a google spreadsheet that represents data models for a campaign/experience and ultimately stores this data into IQ.
The data can then be published into a Json based NoSql database ( currently RethinkDB ) so the client team ( writing the apps ) can use the data in the app for that experience.
At a very high-level, the component allows a dynamic database of model data to be represented in Excel sheets and rows, takes this data and then puts it into a relational database table(s) storing the data in JSON format.
Theorhetically, this kind of functionality should be available as a web interface for no-sql based databases, but it doesn't exist.

## [Sample Spreadsheet](/BrightLine.CMS/_docs/DisneyParksApp_MASTER.xlsx)


# Data models
1. Examples of models include Images, Videos, Locations, Products and are associated with the experience.
2. Each model is stored in specific sheet in the spreadsheet
3. Each model can have multiple fields / properties.
4. Each record or instance of a model is a row in the sheet. e.g. the Images model with 10 images/records.

![Sample Model](/BrightLine.CMS/_docs/_sample_model_sheet.PNG?raw=true)

# Terminology
1. Model : models include Images, Videos, Location, Products 
2. Model Schema: stores all the properties of a specific model ( analogy : Table schema )
3. Model Records: Collection of instances of a model e.g. 10 images ( analogy : Table rows )
4. Model Property: A property of a model. e.g. Image.FileName ( e.g. Table column )
5. Model Property Type: The data type of a property of a model. E.g. ( FileName = text, IsFeatured = bool ) see Property Types below.
6. Model filter: An optional filter setup for each model in the "MODELS" sheet. The filter specifies whether or not a record for that model should be extracted based on a column and it's value.
7. Settings

# Setup
1. The spreadsheet nameed "KEY" is required and represents all the enumerated lookup values used
2. The spreadsheet named "MODELS" is required and tells the importer which models to load and how to filter records of the model.

# Naming conventions
When the data is imported from excel and ultimately converted to json data, the names of properties ( fields ) in excel are converted from the raw format to a camel case format.

1. Spaces are removed and the 1st char of 2nd word is captialized
2. Only chars and numbers are kept
3. underscores are kept

Example:
'HEADER SRC' becomes 'headerSrc'
'HEADER_SRC' becomes 'header_src'
'HEADER =?(' becomes 'header'


# Property types
1. text
2. text-50 ( or any number after indicates limiting text to that number of chars )
3. number
4. datetime
5. true/false


# Lists
1. List:text
2. List:number
3. List:true/false
4. List:datetime


# References
1. ref-category  ( example of referencing a Category lookup value in the "KEY" sheet )
2. ref-video     ( example of referencing another model by it KEY field in it's sheet )


# List/References
1. list:ref-video



# Core classes / entities
Entities/classes are designed to mimick a database table schema ( This means the concept of a "table" being composed of a schema with columns and data rows translates directly to concepts and classes in the CMS )
NOTE: The cms entities listed below are much more convenient for storing and processing the data from excel. They are ultimately converted to the entity framework models for storing the data into IQ.

| databse concept | cms concept | cms entity | model in entity framework |
|:------ |:------  |:------ |:------  |
|table           | model 			| DataModel          | CampaignContentDataModel | 
|table schema    | model schema     | DataModelSchema    | N/A| 
|table column    | model property   | DataModelProperty  | CampaignContentDataModelProperty | 
|table row(s)    | model instance(s)| DataModelRecords   | CampaignContentDataModelInstance | 



# CMS Database tables  ( for metadata / lookups )
| Table Name						| Purpose |
|:----------------- | :-------------- |
| CampaignContentModelBaseType		| e.g. ( image, video, copy    ) = high-level description of what the model is |
| CampaignContentModelType			| e.g. ( how-to, entertainment ) = business description of what the model is |
| CampaignContentModelPropertyType	| e.g. ( bool, string, datetime, number ) = supported types for dynamic properties   |


# CMS Database tables ( for campaign data )
| Table Name						| Purpose |
|:----------------- | :-------------- |
| CampaignContentSchema				| Stores all the model names, each models properties, and all lookup values.  **this can potentially be used to build up a web UI from existing schemas** |
| CampaignContentModel				| Stores the name, basetype, of each model |
| CampaignContentModelInstance		| Stores each record / instance of each of model   |
| CampaignContentModelProperty		| Stores each property of each model that is used for reporting ( marked as meetadata:server ) |
| CampaignContentModelPropertyValue	| Represents the vertical table storing all the property values ( of properties marked as metadata - for reporting ) of each row  |


# Query to retrieve all the schema data
```sql
------------------------------------------------------------------------------------------------
-- @name: CMS Data queries
-- @desc: Shows metadata, schema, models, instances, proeprties and values given a schema id
-- @inputs: schema id ( see below )
------------------------------------------------------------------------------------------------
declare @campid as int
declare @schemaid as int
declare @showDeleted as bit

set @showDeleted = 0
set @schemaid = 14

-- Campaign
select @campid = Campaign_Id from CampaignContentSchema where id = @schemaid
select * from campaign where id = @campid

-- CMS lookup values : metadata ( format / type / property types ( bool, datetime, etc ) )
select * from CampaignContentModelBaseType
select * from CampaignContentModelType
select * from CampaignContentModelPropertyType 

-- CMS data ( schema, models, instances, server properties, vertical properties )
select * from CampaignContentSchema where id = @schemaid
select * from CampaignContentModel where Schema_Id = @schemaid
select * from CampaignContentModelInstance where Schema_Id = @schemaid
select * from CampaignContentModelProperty where model_id in ( select id from CampaignContentModel where Schema_Id = @schemaid )
select * from CampaignContentModelPropertyValue where model_id in ( select id from CampaignContentModel where Schema_Id = @schemaid )

```


# Query to delete all the schema data
```sql
------------------------------------------------------------------------------------------------
-- @name: CMS Data delete sql
-- @desc: deletes all the cms data for a given schema
-- @inputs: schema id ( see below )
------------------------------------------------------------------------------------------------
declare @expid as int
declare @campid as int
declare @schemaid as int
declare @showDeleted as bit

set @showDeleted = 0
select @schemaid = 24
select @campid = Campaign_Id from CampaignContentSchema where id = @schemaid

-- Delete cms data 
delete from CampaignContentModelPropertyValue where model_id in ( select id from CampaignContentModel where Schema_Id = @schemaid )
delete from CampaignContentModelProperty where model_id in ( select id from CampaignContentModel where Schema_Id = @schemaid )
delete from CampaignContentModelInstance where Schema_Id = @schemaid
delete from CampaignContentModel where Schema_Id = @schemaid
update CampaignContentSchema set DataModelsRaw = null, DataModelsPublished = null, TotalModels = 0, TotalLookups = 0, IsPublished = 0, LastPublishedDate = null, ChangeComment = null, LastPublishEnvironment = null where id = @schemaid

-- Show the campaign and schema ( they are never deleted )
select * from campaign where id = @campid
select * from CampaignContentSchema where id = @schemaid

```


# Workflow
### 1. Load metadata from excel:
- Ensure that both "MODELS" and "KEY" sheets exists as they are basically setup sheets that drive the extraction
- Read the "MODELS" sheet to determine what models exist and what sheet each model maps to and if there is a model filter
- Read the "KEY" sheet to determine what lookups exist and load all of the lookups
- Load the model schema/metadata from it's respective sheet. This includes all the properties the model has.
- Validate the model scheama and it's properties are correct
- Load the model data from it's respective sheet, facotring in it's potential filter.

### 2. Data conversion ( from raw excel data )
- Convert each model's data from the it's raw excel value represented mostly as strings, into their actual types ( numbers, bool, dates, lists of numbers )

**refer to**:
AppImportDataConverter.cs


### 3. Data validation ( lists, lookups, references )
- Cross validate the model data ( e.g. ensure references to valid keys of other models, validate references to lookup values )
  For example, a product has a reference to an image via by specifiing the KEY value of the image. So confirm the key value exists in the image model rows.
- Validate each model ( in isolation ) - eg. required values, data value matches it's property type. ( e.g. a number is a number and a date is correct date )
- Validate each model's reference to another model / lookup ( relationships )


**refer to**:
- AppSchemaValidator.cs
- DataModelSchemaValidator.cs
- DataModelReferenceValidator.cs
- DataModelPropertyValidator.cs


### 4. Saving data to IQ.
Currently the the data from the spreadsheet, once extracted is saved and serialized to JSON data and stored in the database tables listed above.
Saving to iq involves a few steps. These steps have been "wrapped" into modular commands using the "command pattern".

**refer to**:
CmsService.ImportSpreadSheet ( calls each of the respective commands )
BrightLine.CMS\Commands


### 5. Publishing data from IQ to no-sql database
Currently the the data from the spreadsheet, once extracted is saved and serialized to JSON data and stored in the database tables listed above.
The JSON format for each record is ultimately what the client team can "consume" to load the data for the experience.
However, all this JSON data is only available in the IQ database. There is a "publish" command that takes this json data from IQ and transfers it to the NoSql database used ( rethinkdb )

