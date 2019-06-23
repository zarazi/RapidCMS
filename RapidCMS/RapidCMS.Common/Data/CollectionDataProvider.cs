﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RapidCMS.Common.Extensions;
using RapidCMS.Common.Models.DTOs;
using RapidCMS.Common.Models.Metadata;

namespace RapidCMS.Common.Data
{

    internal class CollectionDataProvider : IRelationDataCollection
    {
        private readonly IRepository _repository;
        private readonly Type _relatedEntityType;
        private readonly IPropertyMetadata? _repositoryParentIdProperty;
        private readonly IPropertyMetadata _idProperty;
        private readonly IExpressionMetadata _labelProperty;

        private IEntity? _entity;

        private List<IElement>? _elements;
        private List<IElement>? _relatedElements;
        private ICollection<object>? _relatedIds;

        public CollectionDataProvider(IRepository repository, Type relatedEntityType, IPropertyMetadata? repositoryParentIdProperty, IPropertyMetadata idProperty, IExpressionMetadata labelProperty)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _relatedEntityType = relatedEntityType ?? throw new ArgumentNullException(nameof(relatedEntityType));
            _repositoryParentIdProperty = repositoryParentIdProperty;
            _idProperty = idProperty ?? throw new ArgumentNullException(nameof(idProperty));
            _labelProperty = labelProperty ?? throw new ArgumentNullException(nameof(labelProperty));
        }

        public async Task SetEntityAsync(IEntity entity)
        {
            _entity = entity;

            var parentId = _repositoryParentIdProperty?.Getter.Invoke(_entity) as string;
            var entities = await _repository._GetAllAsObjectsAsync(parentId);

            _elements = entities
                .Select(entity => (IElement)new ElementDTO
                {
                    Id = _idProperty.Getter(entity),
                    Label = _labelProperty.StringGetter(entity)
                })
                .ToList();
        }

        public async Task SetRelationMetadataAsync(IEntity entity, IPropertyMetadata collectionProperty)
        {
            var data = collectionProperty.Getter(entity);

            await SetEntityAsync(entity);

            if (data is ICollection<IEntity> entityCollection)
            {
                _relatedIds = entityCollection.Select(x => (object)x.Id).ToList();
            }
            else if (data is ICollection<object> objectCollection)
            {
                _relatedIds = objectCollection;
            }
            else if (data is IEnumerable enumerable)
            {
                var list = new List<object>();
                foreach (var element in enumerable)
                {
                    list.Add(element);
                }
                _relatedIds = list;
            }
            else
            {
                throw new InvalidOperationException("Failed to convert relation property to ICollection<object>");
            }

            UpdateRelatedElements();
        }

        private void UpdateRelatedElements()
        {
            _relatedElements = _elements?.Where(x => _relatedIds?.Contains(x.Id) ?? false).ToList();
        }

        public Task<IEnumerable<IElement>> GetAvailableElementsAsync()
        {
            return Task.FromResult(_elements ?? Enumerable.Empty<IElement>());
        }

        public Task<IEnumerable<IElement>> GetRelatedElementsAsync()
        {
            var relatedElements = (_relatedIds != null && _elements != null)
                ? _elements.Where(x => _relatedIds.Contains(x.Id))
                : Enumerable.Empty<IElement>();

            return Task.FromResult(relatedElements);
        }

        public Task AddElementAsync(IElement option)
        {
            _relatedElements?.Add(_elements.First(x => x.Id == option.Id));

            return Task.CompletedTask;
        }
        public Task RemoveElementAsync(IElement option)
        {
            return Task.FromResult(_relatedElements?.RemoveAll(x => x.Id == option.Id));
        }

        public Task SetElementAsync(IElement option)
        {
            _relatedElements?.Clear();
            _relatedElements?.Add(_elements.First(x => x.Id == option.Id));

            return Task.CompletedTask;
        }

        public IEnumerable<IElement> GetCurrentRelatedElements()
        {
            return _relatedElements ?? Enumerable.Empty<IElement>();
        }

        public Type GetRelatedEntityType()
        {
            return _relatedEntityType ?? typeof(object);
        }
    }
}
