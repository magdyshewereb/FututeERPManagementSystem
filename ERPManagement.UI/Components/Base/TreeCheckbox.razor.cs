using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Diagnostics;

namespace ERPManagement.UI.Components.Base
{
    public partial class TreeCheckbox<TItem> : ComponentBase
    {
        [CascadingParameter]
        public List<TItem> lstItems { get; set; }
        public List<TItem> lstChildItems { get; set; }
        [CascadingParameter]
        public List<int> lstItemIDs { get; set; }
        [Parameter]
        public int ParentId { get; set; }
        public List<int> lstIndeterminateIDs { get; set; } = new();
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
        public bool isNavMode { get; set; }
        [Parameter]
        public EventCallback<TItem> OnItemCheckedCallback { get; set; }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && lstIndeterminateIDs.Count == 0)
            {
                for (int i = 0; i < lstItemIDs.Count; i++)
                {
                    TItem item = lstItems.FirstOrDefault(x => x.GetType().GetProperty(IDCol).GetValue(x, null).ToString() == lstItemIDs[i].ToString());
                    while (int.Parse(GetPropertyValue(item, ParentIDCol).ToString()) > 0)
                    {
                        item = lstItems.FirstOrDefault(i => i.GetType().GetProperty(IDCol).GetValue(i, null).ToString() == item.GetType().GetProperty(ParentIDCol).GetValue(item, null).ToString());
                        if (!lstItemIDs.Contains(int.Parse(GetPropertyValue(item, IDCol).ToString())))
                        {
                            Indeterminated_CheckedState(item);
                        }
                    }
                }
                js.InvokeVoidAsync("CheckBoxTree.setChecked", lstItemIDs);
                js.InvokeVoidAsync("CheckBoxTree.setIndeterminate", lstIndeterminateIDs);
            }
        }


        Dictionary<int, bool> expandedParent = new();

        bool IsParentExpanded(int itemID) =>
        expandedParent.ContainsKey(itemID) && expandedParent[itemID];
        void ToggleParents(int ItemID)
        {
            if (!isNavMode)
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
        private void NodeChecked(TItem item)
        {
            if (lstItemIDs != null && (lstItemIDs.Contains(int.Parse(GetPropertyValue(item, IDCol).ToString())) || lstIndeterminateIDs.Contains(int.Parse(GetPropertyValue(item, IDCol).ToString()))))
            {
                lstItemIDs.Remove(int.Parse(GetPropertyValue(item, IDCol).ToString()));
                lstIndeterminateIDs.Remove(int.Parse(GetPropertyValue(item, IDCol).ToString()));
                js.InvokeVoidAsync("CheckBoxTree.setUnchecked", GetPropertyValue(item, IDCol));
                UnCheckAllChilds(item);
                // get Parent Item
                item = lstItems.FirstOrDefault(i => i.GetType().GetProperty(IDCol).GetValue(i, null).ToString() == item.GetType().GetProperty(ParentIDCol).GetValue(item, null).ToString());
                if (item != null)
                {
                    Indeterminated_UnCheckedState(item);
                }
            }
            else
            {
                lstItemIDs.Add(int.Parse(GetPropertyValue(item, IDCol).ToString()));
                CheckAllChilds(item);
                while (int.Parse(GetPropertyValue(item, ParentIDCol).ToString())> 0)
                {
                    // get Parent Item
                    item = lstItems.FirstOrDefault(i => i.GetType().GetProperty(IDCol).GetValue(i, null).ToString() == item.GetType().GetProperty(ParentIDCol).GetValue(item, null).ToString());
                    if (!lstItemIDs.Contains(int.Parse(GetPropertyValue(item, IDCol).ToString())))
                    {
                        //ItemIDs.Add(int.Parse(GetPropertyValue(item, IDCol).ToString()));
                        Indeterminated_CheckedState(item);
                    }
                }
            }
            js.InvokeVoidAsync("CheckBoxTree.setChecked", lstItemIDs);
            js.InvokeVoidAsync("CheckBoxTree.setIndeterminate", lstIndeterminateIDs);

        }
        private void CheckAllChilds(TItem item)
        {
            if (bool.Parse(GetPropertyValue(item, IsMainCol).ToString()))
            {
                foreach (TItem child in lstItems.Where(a => GetPropertyValue(a, ParentIDCol) == GetPropertyValue(item, IDCol)).ToList())
                {
                    if (!lstItemIDs.Contains(int.Parse(GetPropertyValue(child, IDCol).ToString())))
                    {
                        lstItemIDs.Add(int.Parse(GetPropertyValue(child, IDCol).ToString()));
                    }
                    CheckAllChilds(child);
                }
            }
        }
        private void UnCheckAllChilds(TItem item)
        {
            if (bool.Parse(GetPropertyValue(item, IsMainCol).ToString()))
            {
                foreach (TItem child in lstItems.Where(a => GetPropertyValue(a, ParentIDCol) == GetPropertyValue(item, IDCol)).ToList())
                {
                    if (lstItemIDs != null && lstItemIDs.Contains(int.Parse(GetPropertyValue(child, IDCol).ToString())))
                    {
                        lstItemIDs.Remove(int.Parse(GetPropertyValue(child, IDCol).ToString()));
                        UnCheckAllChilds(child);
                    }
                    else if (lstIndeterminateIDs != null && lstIndeterminateIDs.Contains(int.Parse(GetPropertyValue(child, IDCol).ToString())))
                    {
                        lstIndeterminateIDs.Remove(int.Parse(GetPropertyValue(child, IDCol).ToString()));
                        js.InvokeVoidAsync("CheckBoxTree.setUnchecked", GetPropertyValue(child, IDCol));
                        UnCheckAllChilds(child);
                    }

                }
            }
        }
        private void Indeterminated_UnCheckedState(TItem parent)
        {
            List<TItem> childs = lstItems.Where(i => i.GetType().GetProperty(ParentIDCol).GetValue(i, null).ToString() == parent.GetType().GetProperty(IDCol).GetValue(parent, null).ToString()).ToList();
            bool isIndetermined = false;
            for (int i = 0; i < childs.Count; i++)
            {
                if (lstItemIDs.Contains(int.Parse(GetPropertyValue(childs[i], IDCol).ToString())) || lstIndeterminateIDs.Contains(int.Parse(GetPropertyValue(childs[i], IDCol).ToString())))
                {
                    isIndetermined = true;
                    break;
                }
            }
            if (isIndetermined)
            {
                if (!lstIndeterminateIDs.Contains(int.Parse(GetPropertyValue(parent, IDCol).ToString())))
                {
                    lstIndeterminateIDs.Add(int.Parse(GetPropertyValue(parent, IDCol).ToString()));
                    lstItemIDs.Remove(int.Parse(GetPropertyValue(parent, IDCol).ToString()));
                }
            }
            else
            {
                lstItemIDs.Remove(int.Parse(GetPropertyValue(parent, IDCol).ToString()));
                lstIndeterminateIDs.Remove(int.Parse(GetPropertyValue(parent, IDCol).ToString()));
                js.InvokeVoidAsync("CheckBoxTree.setUnchecked", GetPropertyValue(parent, IDCol));
            }
            parent = lstItems.FirstOrDefault(i => i.GetType().GetProperty(IDCol).GetValue(i, null).ToString() == parent.GetType().GetProperty(ParentIDCol).GetValue(parent, null).ToString());
            if (parent != null)
            {
                Indeterminated_UnCheckedState(parent);
            }
        }
        private void Indeterminated_CheckedState(TItem parent)
        {
            List<TItem> childs = lstItems.Where(i => i.GetType().GetProperty(ParentIDCol).GetValue(i, null).ToString() == parent.GetType().GetProperty(IDCol).GetValue(parent, null).ToString()).ToList();
            bool isIndetermined = false;
            for (int i = 0; i < childs.Count; i++)
            {
                if (!lstItemIDs.Contains(int.Parse(GetPropertyValue(childs[i], IDCol).ToString())))
                {
                    isIndetermined = true;
                    break;
                }
            }
            if (isIndetermined)
            {
                if (!lstIndeterminateIDs.Contains(int.Parse(GetPropertyValue(parent, IDCol).ToString())))
                {
                    lstIndeterminateIDs.Add(int.Parse(GetPropertyValue(parent, IDCol).ToString()));
                    lstItemIDs.Remove(int.Parse(GetPropertyValue(parent, IDCol).ToString()));
                }
            }
            else
            {
                if (!lstItemIDs.Contains(int.Parse(GetPropertyValue(parent, IDCol).ToString())))
                {
                    lstItemIDs.Add(int.Parse(GetPropertyValue(parent, IDCol).ToString()));
                    lstIndeterminateIDs.Remove(int.Parse(GetPropertyValue(parent, IDCol).ToString()));
                    js.InvokeVoidAsync("CheckBoxTree.setUnchecked", GetPropertyValue(parent, IDCol));

                }
            }
        }
        private bool GetChecked(TItem item)
        {
            if (lstItemIDs != null && lstItemIDs.Contains(int.Parse(GetPropertyValue(item, IDCol).ToString())))
            {
                    return true;
            }
            return false;
        }
        private bool GetIndetermine(TItem item)
        {
            if (lstIndeterminateIDs != null && lstIndeterminateIDs.Contains(int.Parse(GetPropertyValue(item, IDCol).ToString())))
            {
                return true;
            }
            return false;
        }
        #region reflection methodes to get your property type, propert value and also set property value 
        protected string GetPropertyValue(TItem item, string Property)
        {

            if (item != null)
            {
                return item.GetType().GetProperty(Property).GetValue(item, null)==null?DBNull.Value.ToString(): item.GetType().GetProperty(Property).GetValue(item, null).ToString();
            }
            return "";
        }
        #endregion
    }
}
