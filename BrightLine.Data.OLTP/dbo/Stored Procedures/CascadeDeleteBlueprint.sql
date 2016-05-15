------------------------------------------------------------------------------------------------------------------------
-- This procedure will delete a blueprint and all of its dependent entities
------------------------------------------------------------------------------------------------------------------------
CREATE procedure [dbo].[CascadeDeleteBlueprint]	
	@BlueprintId int
as
begin
	set nocount on;

	delete from page where Feature_Id in (select id from feature where Blueprint_Id in (select id from blueprint where id in(@BlueprintId)))
	delete from CmsSetting where Feature_Id in (select id from feature where Blueprint_Id in (select id from blueprint where id in(@BlueprintId)))
	delete from cmsmodelinstance where model_id in (select id from CmsModel where Feature_Id in (select id  from feature where Blueprint_Id in (select id from blueprint where id in(@BlueprintId))))
	delete from CmsModel where Feature_Id in (select id  from feature where Blueprint_Id in (select id from blueprint where id in(@BlueprintId)))
	delete from feature where Blueprint_Id in (select id from blueprint where id in(@BlueprintId))
	delete from FileTypeValidation where Validation_Id in (select id from validation where CmsField_Id in (select id from CmsField where CmsSettingDefinition_Id in (select id from CmsSettingDefinition where Blueprint_Id in (@BlueprintId))) )
	delete from validation where CmsField_Id in (select id from CmsField where CmsSettingDefinition_Id in (select id from CmsSettingDefinition where Blueprint_Id in (@BlueprintId))) 
	delete from CmsField where CmsSettingDefinition_Id in (select id from CmsSettingDefinition where Blueprint_Id in (@BlueprintId))
	delete from CmsSetting where CmsSettingDefinition_Id in (select id from CmsSettingDefinition where Blueprint_Id in (@BlueprintId))
	delete from CmsSettingDefinition where Blueprint_Id in (@BlueprintId)


	delete from FileTypeValidation where Validation_Id in (select id from validation where CmsField_Id in (select id from CmsField where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId))))
	delete from validation where CmsField_Id in (select id from CmsField where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId)))
	delete from CmsField where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId))
	delete from CmsField where CmsRef_Id in (select id from CmsRef where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId)))
	delete from CmsRef where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId))
	delete from CmsModelInstanceFieldValue where CmsModelInstanceField_Id in (select id from CmsModelInstanceField where ModelInstance_Id in (select id from CmsModelInstance where Model_Id in (select id from CmsModel where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId)))))
	delete from CmsModelInstanceField where ModelInstance_Id in (select id from CmsModelInstance where Model_Id in (select id from CmsModel where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId))))
	delete from CmsModelInstance where Model_Id in (select id from CmsModel where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId)))
	delete from CmsModel where CmsModelDefinition_Id in (select id from CmsModelDefinition where Blueprint_Id in (@BlueprintId))
	delete from CmsModelDefinition where Blueprint_Id in (@BlueprintId)
	delete from Page where PageDefinition_Id in (select id from PageDefinition where Blueprint_Id in (@BlueprintId))
	delete from CmsRef where PageDefinition_Id in (select id from PageDefinition where Blueprint_Id in (@BlueprintId))
	delete from PageDefinition where Blueprint_Id in (@BlueprintId)
	delete from blueprint  where id in(@BlueprintId)

end
GO


