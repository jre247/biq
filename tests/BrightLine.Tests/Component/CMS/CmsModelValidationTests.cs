using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using NUnit.Framework;
using System.Collections.Generic;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class CmsModelValidationTests
	{
		private CmsModel _choiceModel { get; set;}
		private CmsModel _questionModel { get; set; }
		private CmsModel _resultModel { get; set; }

		[SetUp]
		public void Setup()
		{
			_choiceModel = new CmsModel
			{
				Id = 1,
				Display = "Choice",
				ModelInstances = new List<CmsModelInstance>{
					new CmsModelInstance{
						Model = new CmsModel{
							Id = 1,
							Display = "Choice"
						},
						Display = "I will you be going to McDonalds later"
					},
					new CmsModelInstance{
						Model = new CmsModel{
							Id = 1,
							Display = "Choice"
						},
						Display = "I will you be going to Burger King later"
					},
					new CmsModelInstance{
						Model = new CmsModel{
							Id = 1,
							Display = "Choice"
						},
						Display = "I will you be going to Roy Rogers later"
					}
				},
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = 1,
					Name = "choice",
					Fields = new List<CmsField>
					{
						new CmsField{
							Id = 1,
							Name = "thumbnailSrc",
							Description = "Sprite to use for the choice",
							List = false,
							Display = "Choice Image",
							Type = new FieldType{
								Id = 1,
								Name = "image"								
							}
						}
					}
				}
			};

			_questionModel = new CmsModel
			{
				Id = 2,
				Display = "Question",
				ModelInstances = new List<CmsModelInstance>{
					new CmsModelInstance{
						Model = new CmsModel{
							Id = 1,
							Display = "Question"
						},
						Display = "Where are you going to lunch later?"
					}
				},
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = 2,
					Name = "question",
					Fields = new List<CmsField>
					{
						new CmsField{
							Id = 1,
							Name = "choices",
							Description = "A list of choices to choose from for the question",
							List = true,
							Display = "Choices",
							Type = new FieldType{
								Id = 2,
								Name = "Ref"								
							}
						},
						new CmsField{
							Id = 1,
							Name = "ordinality",
							Description = "The order for the questions to be displayed",
							List = true,
							Display = "Oridinality",
							Type = new FieldType{
								Id = 4,
								Name = "number"								
							}
						},
						new CmsField{
							Id = 1,
							Name = "headerSrc",
							Description = "Image to use for the header of the feature",
							List = true,
							Display = "Header Image",
							Type = new FieldType{
								Id = 1,
								Name = "image"								
							}
						}
					}
				}
			};

			_resultModel = new CmsModel
			{
				Id = 3,
				Display = "Result",
				ModelInstances = new List<CmsModelInstance>{
					new CmsModelInstance{
						Model = new CmsModel{
							Id = 1,
							Display = "Result"
						},
						Display = "The Restaurant I decided to go to later"
					}
				},
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = 2,
					Name = "question",
					Fields = new List<CmsField>
					{
						new CmsField{
							Id = 1,
							Name = "choices",
							Description = "The choices that if chosen, will lead to this result",
							List = true,
							Display = "Choices",
							Type = new FieldType{
								Id = 2,
								Name = "Ref"								
							}
						},
						new CmsField{
							Id = 1,
							Name = "url",
							Description = "Url to another feature in the microsite",
							List = true,
							Display = "Link Url",
							Type = new FieldType{
								Id = 3,
								Name = "string"								
							}
						},
						new CmsField{
							Id = 1,
							Name = "heroSrc",
							Description = "Hero Image for the result",
							List = true,
							Display = "Hero Image",
							Type = new FieldType{
								Id = 1,
								Name = "image"								
							}
						},
						new CmsField{
							Id = 1,
							Name = "descriptionSrc",
							Description = "Image to use for the result description",
							List = true,
							Display = "Description Image",
							Type = new FieldType{
								Id = 1,
								Name = "image"								
							}
						}
					}
				}
			};
		}

		[TearDown]
		public void TearDown()
		{ }

		
    }
}
