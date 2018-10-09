using System;
using System.Threading.Tasks;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Navigation;

namespace OrchardCore.Lists.AdminNodes
{
    public class ListsAdminNodeDriver : DisplayDriver<MenuItem, ListsAdminNode>
    {
        public override IDisplayResult Display(ListsAdminNode treeNode)
        {
            return Combine(
                View("ListsAdminNode_Fields_TreeSummary", treeNode).Location("TreeSummary", "Content"),
                View("ListsAdminNode_Fields_TreeThumbnail", treeNode).Location("TreeThumbnail", "Content")
            );
        }

        public override IDisplayResult Edit(ListsAdminNode treeNode)
        {
            return Initialize<ListsAdminNodeViewModel>("ListsAdminNode_Fields_TreeEdit", model =>
            {
                model.ContentTypes = treeNode.ContentTypes;
                model.Enabled = treeNode.Enabled;
                model.CustomClasses = string.Join(",", treeNode.CustomClasses);
                model.AddContentTypeAsParent = treeNode.AddContentTypeAsParent;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ListsAdminNode treeNode, IUpdateModel updater)
        {
            var model = new ListsAdminNodeViewModel();

            if (await updater.TryUpdateModelAsync(model, Prefix, x => x.ContentTypes, x => x.Enabled, x => x.CustomClasses, x => x.AddContentTypeAsParent)) {
                treeNode.Enabled = model.Enabled;
                treeNode.ContentTypes = model.ContentTypes;
                treeNode.AddContentTypeAsParent = model.AddContentTypeAsParent;
                treeNode.CustomClasses =  string.IsNullOrEmpty( model.CustomClasses) ?  Array.Empty<string>() : model.CustomClasses.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            };

            return Edit(treeNode);
        }
    }
}