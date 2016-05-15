using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Models
{
	public class DataModels
	{
		private Dictionary<string, DataModel> _lookup;


		public DataModels()
		{
			Models = new List<DataModel>();
            _lookup = new Dictionary<string, DataModel>();
		}


		/// <summary>
		/// Lost of all the instances of all the models.
		/// </summary>
        public List<DataModel> Models;


		
		/// <summary>
		/// Loads the properties into a lookup table.
		/// </summary>
		public void LoadLookup()
		{
            if(_lookup == null)
                _lookup = new Dictionary<string, DataModel>();
			if (Models == null)
				return;

			foreach (var model in Models)
			{
			    LoadLookup(model);
			}
		}


        public void LoadLookup(DataModel model)
        {
            _lookup[model.Name.ToLower()] = model;
            model.Rows.LoadLookup();
        }


        public void Add(DataModel model)
        {
            _lookup[model.Name] = model;
            Models.Add(model);
        }


	    public int Count
	    {
            get { return Models == null ? 0 : Models.Count;  }
	    }


		/// <summary>
		/// Whether or not this has the name supplied.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool HasModel(string name)
		{
			return _lookup.ContainsKey(name);
		}


		/// <summary>
		/// Gets the field with the supplied name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public DataModel GetModel(string name)
		{
			name = name.ToLower();

			if (!HasModel(name))
				return null;
			return _lookup[name];
		}
	}
}
