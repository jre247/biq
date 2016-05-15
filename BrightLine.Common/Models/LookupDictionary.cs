using BrightLine.Common.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	/// <summary>
	/// This class contains the property Lookups, which is a dictionary of various lookups that correspond to various database lookup tables.
	/// The Lookups dictionary's key is the lookup table name, and the value is a dictionary of lookups for this particular database lookup table.
	/// </summary>
	public class LookupsDictionary
	{
		public Dictionary<string, LookupDictionaryItem> Lookups { get; set; }

		public LookupsDictionary(List<LookupItem> lookupItems) 
		{
			if(lookupItems == null)
				return;

			Lookups = new Dictionary<string, LookupDictionaryItem>();

			var lookupItemList = new List<LookupItem>();
			string currentTableName = lookupItems.First().TableName;

			//TODO: get multiple result sets from stored proc and loop through them instead of this manual way of figuring out when each table starts and ends in the single result set
			for (var i = 0; i < lookupItems.Count(); i++)
			{			
				var lookupItem = lookupItems[i];

				if (lookupItem.TableName != currentTableName)
				{
					AddItemToLookupsDictionary(lookupItemList, currentTableName);

					//reset to new list of current lookup
					currentTableName = lookupItem.TableName;
					lookupItemList = new List<LookupItem>();
				}

				lookupItemList.Add(lookupItem);

				//reached end so make sure to add last list of lookups to look ups dictionary
				if (i == lookupItems.Count() - 1)
					AddItemToLookupsDictionary(lookupItemList, currentTableName);
			}
		}

		//we know we're about go move on to new lookup table so add current look up table dictinoary item
		//to lookups dictionary
		private void AddItemToLookupsDictionary(List<LookupItem> lookupItemList, string currentTableName)
		{
			var lookupDictionaryItem = new LookupDictionaryItem(currentTableName, lookupItemList);
			Lookups.Add(currentTableName, lookupDictionaryItem);
		}
	}

	/// <summary>
	/// This class will have 3 propties: 
	///		1) TableName: represents the database table that this Lookup Dictionary corresponds to
	///		2) HashById: this is a dictionary that contains all of the lookups for this particular database table. 
	///		   This dictionary will have the key be the lookup Id, and the value be the lookup name
	///		3) HashByName: this is a dictionary that contains all of the lookups for this particular database table.
	///		   This dictionary will have the key be the lookup name, and the value be the lookup Id
	/// </summary>
	public class LookupDictionaryItem
	{
		private string TableName { get;set;}
		public Dictionary<int, string> HashById { get;set;}
		public Dictionary<string, int> HashByName { get; set; }

		public LookupDictionaryItem(string tableName, List<LookupItem> lookupItems)
		{
			this.TableName = tableName;
			HashById = new Dictionary<int, string>();
			HashByName = new Dictionary<string, int>();

			foreach (var lookupItem in lookupItems)
			{
				if (HashById.ContainsKey(lookupItem.Id))
					IoC.Log.ErrorFormat("Lookups HashById already has a Lookup key of {0} from table {1}", lookupItem.Id, lookupItem.TableName);
				else
					HashById.Add(lookupItem.Id, lookupItem.Name);

				if (HashByName.ContainsKey(lookupItem.Name))
					IoC.Log.ErrorFormat("Lookups HashByName already has a Lookup key of {0} from table {1}", lookupItem.Name, lookupItem.TableName);
				else
					HashByName.Add(lookupItem.Name, lookupItem.Id);
			}
		}
	}

}
