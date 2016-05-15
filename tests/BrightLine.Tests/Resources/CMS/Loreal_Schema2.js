{	
	"App": "Loreal",
	"Desc": "First version of loreal application schema",
	"Lookups":
	[
		{
			"Name": "Season",
			"Values": [ "Winter", "Summer", "Spring", "Fall", "Evergreen" ]
		},
		{		
			"Name": "Brand",
			"Values": ["GAR","MNY","LOP","REDK","LANC","RL","YSL","LRP","SSC"]
		},
		{
			"Name": "ColorFamily",
			"Values": ["Red","Pink","Purple","Orange","Green","Blue","Yellow","Black","Brown","White","Gold","Silver"]			
		},
		{
			"Name": "Category",
			"Values": ["Eyes","Lips","Hair","Nails","Face","Fragrance"]			
		}
	],
	"Models" :
	[
			{
				"Name": "product",
				"Fields":
				[
					{ "Name": "active", 	  		  	"DataType": "bool", 		    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "goLiveDate", 		  	"DataType": "datetime", 	    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "brand", 	  		  		"DataType": "@brand", 		    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "category",   		  	"DataType": "@category",	    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 0,   "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" },
					{ "Name": "productname", 	  		"DataType": "text", 		    "Required": true,  "DefaultValue": "", 		"RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "desc",		  		  	"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "heroImageFileName",  	"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "popupImageFileName", 	"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "thumbImageFileName", 	"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "productUrlLink", 	  	"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "availableToPurchase",	"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "swatchImageFileName",	"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "associatedVideos",		"DataType": "list:@video",      "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" },
					{ "Name": "newTag", 				"DataType": "values:on,off",    "Required": true,  "DefaultValue": "",      "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "season", 				"DataType": "@season", 		    "Required": true,  "DefaultValue": "",      "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "colorFamily", 			"DataType": "@colorfamily", 	"Required": true,  "DefaultValue": "",      "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
					{ "Name": "socialMessage", 			"DataType": "string", 		    "Required": true,  "DefaultValue": "",      "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }
				]
			}
	]
}