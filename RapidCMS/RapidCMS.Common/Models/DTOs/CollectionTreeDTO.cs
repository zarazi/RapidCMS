﻿using System.Collections.Generic;
using RapidCMS.Common.Enums;
using RapidCMS.Common.Interfaces;

#nullable enable

namespace RapidCMS.Common.Models.DTOs
{
    public class CollectionTreeNodeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<CollectionTreeCollectionDTO> Collections { get; set; }
    }
    public class CollectionTreeRootDTO : CollectionTreeNodeDTO
    {

    }
    
    public class CollectionTreeCollectionDTO
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public List<CollectionTreePathDTO> Path { get; set; }
        public List<CollectionTreeNodeDTO> Nodes { get; set; }
    }

    public class CollectionTreePathDTO
    {
        public string Icon { get; set; }
        public string Path { get; set; }
    }

    public class CollectionListViewDTO
    {
        public List<ButtonDTO> Buttons { get; set; }
        public List<CollectionListViewPaneDTO> ViewPanes { get; set; }
    }

    public class CollectionListViewPaneDTO
    {
        public List<ButtonDTO> Buttons { get; set; }
        public List<PropertyDTO> Properties { get; set; }
        public List<NodeDTO> Nodes { get; set; }
    }

    public class CollectionListEditorDTO
    {
        public List<ButtonDTO> Buttons { get; set; }
        public CollectionListEditorPaneDTO Editor { get; set; }
        public ListEditorType ListEditorType { get; set; }
    }

    public class SubCollectionListEditorDTO
    {
        public string CollectionAlias { get; set; }
        public string Action { get; set; }
    }

    public class CollectionListEditorPaneDTO
    {
        public List<ButtonDTO> Buttons { get; set; }
        public List<PropertyDTO> Properties { get; set; }
        public List<NodeDTO> Nodes { get; set; }
    }

    public class NodeDTO
    {
        public string VariantAlias { get; set; }
        public string? Id { get; set; }
        public string? ParentId { get; set; }

        public List<ButtonDTO> Buttons { get; set; }
        public List<ValueDTO> Values { get; set; }
    }

    public class PropertyDTO
    {
        // TODO: make really unique
        public string Alias { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class NodeEditorDTO
    {
        public string Id { get; set; }
        public string Alias { get; set; }

        public List<ButtonDTO> Buttons { get; set; }
        public List<NodeEditorPaneDTO> EditorPanes { get; set; }
    }

    public class NodeEditorPaneDTO
    {
        public List<(LabelDTO label, ValueDTO value)> Fields { get; set; }
        public List<SubCollectionListEditorDTO> SubCollectionListEditors { get; set; }
    }

    public class LabelDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    // TODO: change to complete field / property DTO
    public class ValueDTO
    {
        // TODO: make really unique
        public string Alias { get; set; }

        public EditorType Type { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public bool IsReadonly { get; set; }

        public IDataProvider? DataProvider { get; set; }
    }

    public class OptionDTO : IOption
    {
        public string Id { get; set; }
        public string Label { get; set; }
    }

    public class ButtonDTO
    {
        public string ButtonId { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public bool ShouldConfirm { get; set; }

        public string Alias { get; set; }
    }

    public class ActionResult
    {
        public string Action { get; set; }
        public string Data { get; set; }
    }
}
