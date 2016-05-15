using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Utility;
using BrightLine.Core;
using BrightLine.Utility.Validation;

namespace BrightLine.Common.Models.Validators
{
    public class EntityNameValidator<T> : ValidatorBase where T : EntityBase
    {
        protected IDictionary<string, T> _itemLookup;
        protected bool _isInitialized;
        protected readonly string _itemToValidate;
        protected string ENTITY_NAME = "CampaignContentModelType";
        protected string ENTITY_FRIENDLY_NAME = "model type";
        protected Func<IDictionary<string, T>> _fetcher;


        public EntityNameValidator()
        {
        }


        public EntityNameValidator(string modelType)
        {
            _itemToValidate = modelType;
        }


        public EntityNameValidator(string entityFriendlyName, string itemToValidate, Func<IDictionary<string, T>> fetcher)
        {
            ENTITY_NAME = typeof (T).Name;
            ENTITY_FRIENDLY_NAME = entityFriendlyName;
            _itemToValidate = itemToValidate;
            _fetcher = fetcher;
        }


        /// <summary>
        /// Validate the label and method using values supplied in constructor
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            var result = Validate(_itemToValidate);
            return result.Success;
        }


        /// <summary>
        /// Validate the adevent label, method.
        /// </summary>
        /// <param name="itemToValidate"></param>
        /// <returns></returns>
        public BoolMessageItem<T> Validate(string itemToValidate)
        {
            var result = ValidateInternal(itemToValidate);
            if (!result.Success)
                base.CollectError(ENTITY_NAME, result.Message);
            return result;
        }


        /// <summary>
        /// Valdiates the ad type.
        /// </summary>
        /// <param name="itemText"></param>
        /// <returns></returns>
        public BoolMessageItem<T> ValidateInternal(string itemText)
        {
            InitLookups();
            ClearErrors();

            // 1. Check for empty and delimeter
            if (string.IsNullOrEmpty(itemText))
                return new BoolMessageItem<T>(false, ENTITY_FRIENDLY_NAME + " not supplied", default(T));

            // 2. Now confirm they are all valid.
            if (!_itemLookup.ContainsKey(itemText))
                return new BoolMessageItem<T>(false, ENTITY_FRIENDLY_NAME + " supplied : '" + itemText + "' is invalid", default(T));

            T match = _itemLookup[itemText];
            return new BoolMessageItem<T>(true, string.Empty, match);
        }


        public void InitLookups()
        {
            if (_isInitialized)
                return;

            _itemLookup = _fetcher();
            _isInitialized = true;
        }
    }
}
