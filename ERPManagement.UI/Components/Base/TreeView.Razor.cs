using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;

namespace ERPManagement.UI.Components.Base
{
    public partial class TreeView <TItem>: ComponentBase
    {
        [CascadingParameter]
        public List<TItem> lstItems { get; set; }
        [Parameter]
        public int ParentId { get; set; }
            
        //Table column
        [Parameter]
        public string IDCol { get; set; }
        [Parameter]
        public string NoCol { get; set; }
        [Parameter]
        public string NameCol { get; set; }
        [Parameter]
        public string NameEnCol { get; set; }
        [Parameter]
        public string ParentIDCol { get; set; }
        [Parameter]
        public string IsMainCol { get; set; }
        [Parameter]
        public string ItemLevelCol { get; set; }
        [Parameter]
        public string AdditionalCol1 { get; set; }
        [Parameter]
        public string AdditionalCol2 { get; set; }
        [Parameter]
        public string TableName { get; set; }

        List<string> ItemIDs = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool isNaveMode { get; set; }
        [Parameter]
        public EventCallback<TItem> OnItemSelectedCallback { get; set; }
        Dictionary<int, bool> expandedParent = new();

        bool IsParentExpanded(int itemID) =>
        expandedParent.ContainsKey(itemID) && expandedParent[itemID];
        void ToggleParents(int ItemID)
        {
            if (isNaveMode)
            {
                if (!expandedParent.ContainsKey(ItemID))
                {
                    expandedParent[ItemID] = true;
                }
                else
                {
                    expandedParent[ItemID] = !expandedParent[ItemID];
                }
            }
        }
        private void SelectNode(TItem item)
        {
            if (isNaveMode)
            {
                OnItemSelectedCallback.InvokeAsync(item);
                if (ItemIDs.Count > 0)
                {
                    js.InvokeVoidAsync("TreeView.setHighlightDefault");
                    ItemIDs.Clear();
                }

                while (item != null)
                {
                    ItemIDs.Add("Node" + GetPropertyValue(item, IDCol));
                    item = lstItems.FirstOrDefault(i => i.GetType().GetProperty(IDCol).GetValue(i, null).ToString() == item.GetType().GetProperty(ParentIDCol).GetValue(item, null).ToString());
                }
                js.InvokeVoidAsync("TreeView.highlightSelected", ItemIDs, NoCol, 2);
            }
        }
        //private void AddToSelected(TItem item)
        //{
        //    selectedIDs.Clear();
        //    while (item != null)
        //    {
        //        selectedIDs.Add(int.Parse(GetPropertyValue(item, IDCol).ToString()));
        //        item = lstItems.SingleOrDefault(i => i.GetType().GetProperty(IDCol).GetValue(i, null).ToString() == item.GetType().GetProperty(ParentIDCol).GetValue(item, null).ToString());
        //    }
        //}
        #region reflection methodes to get your property type, propert value and also set property value 
        protected string GetPropertyValue(TItem item, string Property)
        {

            if (item != null)
            {
                return item.GetType().GetProperty(Property).GetValue(item, null)==null?DBNull.Value.ToString(): item.GetType().GetProperty(Property).GetValue(item, null).ToString();
            }
            return "";
        }

        protected void SetPropertyValue<T>(TItem item, string Property, T value)
        {
            if (item != null)
            {
                item.GetType().GetProperty(Property).SetValue(item, value);
            }
        }

        protected string GetPropertyType(TItem item, string Property)
        {

            if (item != null)
            {
                return item.GetType().GetProperty(Property).PropertyType.Name;

            }
            return null;
        }
        #endregion
    }
}
