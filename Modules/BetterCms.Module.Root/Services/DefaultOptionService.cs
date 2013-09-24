﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Models;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Providers;
using BetterCms.Module.Root.ViewModels.Option;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Services
{
    public class DefaultOptionService : IOptionService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOptionService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultOptionService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Merges options and values and returns one list with option value view models for edit (values are returned as strings).
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        public List<OptionValueEditViewModel> GetMergedOptionValuesForEdit(IEnumerable<IOption> options, IEnumerable<IOption> optionValues)
        {
            var optionModels = new List<OptionValueEditViewModel>();

            if (optionValues != null)
            {
                foreach (var optionValue in optionValues.Distinct())
                {
                    IOption option = null;
                    if (options != null)
                    {
                        option = options.FirstOrDefault(
                            f => f.Key.Trim().Equals(optionValue.Key.Trim(), StringComparison.OrdinalIgnoreCase) && f.Type == optionValue.Type);
                    }

                    var optionViewModel = new OptionValueEditViewModel
                                              {
                                                  Type = optionValue.Type,
                                                  CustomOption = optionValue.CustomOption != null ? new CustomOptionViewModel
                                                        {
                                                            Identifier = optionValue.CustomOption.Identifier,
                                                            Title = optionValue.CustomOption.Title
                                                        } : null,
                                                  OptionKey = optionValue.Key.Trim(),
                                                  OptionValue = optionValue.Value,
                                                  OptionDefaultValue = option != null ? option.Value : null,
                                                  UseDefaultValue = false
                                              };

                    if (option == null)
                    {
                        optionViewModel.CanEditOption = true;
                    }

                    optionModels.Add(optionViewModel);
                }
            }

            if (options != null)
            {
                foreach (var option in options.Distinct())
                {
                    if (!optionModels.Any(f => f.OptionKey.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        optionModels.Add(
                            new OptionValueEditViewModel
                                {
                                    Type = option.Type,
                                    CustomOption = option.CustomOption != null ? new CustomOptionViewModel
                                            {
                                                Identifier = option.CustomOption.Identifier,
                                                Title = option.CustomOption.Title
                                            } : null,
                                    OptionKey = option.Key.Trim(),
                                    OptionValue = option.Value,
                                    OptionDefaultValue = option.Value,
                                    UseDefaultValue = true
                                });
                    }
                }
            }

            return optionModels.OrderBy(o => o.OptionKey).ToList();
        }

        /// <summary>
        /// Merges options and values and returns one list with option value view models for use (values are returned as objects).
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        public List<IOptionValue> GetMergedOptionValues(IEnumerable<IOption> options, IEnumerable<IOption> optionValues)
        {
            var optionModels = new List<IOptionValue>();

            if (optionValues != null)
            {
                foreach (var optionValue in optionValues.Distinct())
                {
                    var value = GetValueSafe(optionValue);

                    var optionViewModel = new OptionValueViewModel { Type = optionValue.Type, OptionKey = optionValue.Key.Trim(), OptionValue = value };
                    optionModels.Add(optionViewModel);
                }
            }

            if (options != null)
            {
                foreach (var option in options.Distinct())
                {
                    if (!optionModels.Any(f => f.Key.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        var value = GetValueSafe(option);

                        var optionViewModel = new OptionValueViewModel { Type = option.Type, OptionKey = option.Key.Trim(), OptionValue = value };
                        optionModels.Add(optionViewModel);
                    }
                }
            }

            return optionModels.OrderBy(o => o.Key).ToList();
        }

        /// <summary>
        /// Saves the option values: adds new option values and empty values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="optionViewModels">The option view models.</param>
        /// <param name="savedOptionValues">The saved options.</param>
        /// <param name="parentOptions">The parent options.</param>
        /// <param name="entityCreator">The entity creator.</param>
        public void SaveOptionValues<TEntity>(
            IEnumerable<OptionValueEditViewModel> optionViewModels,
            IEnumerable<TEntity> savedOptionValues,
            IEnumerable<IOption> parentOptions,
            Func<TEntity> entityCreator) where TEntity : Entity, IOption
        {
            if (optionViewModels == null)
            {
                optionViewModels = new List<OptionValueEditViewModel>();
            }

            ValidateOptionKeysUniqueness(optionViewModels);

            var customOptions = LoadAndValidateCustomOptions(optionViewModels);

            if (savedOptionValues != null)
            {
                savedOptionValues.Where(sov => optionViewModels.All(ovm => ovm.OptionKey != sov.Key)).ToList().ForEach(del => repository.Delete(del));
            }

            foreach (var optionViewModel in optionViewModels)
            {
                TEntity savedOptionValue = null;
                if (savedOptionValues != null)
                {
                    savedOptionValue = savedOptionValues.FirstOrDefault(f => f.Key.Trim().Equals(optionViewModel.OptionKey.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (!optionViewModel.UseDefaultValue)
                {
                    if (savedOptionValue == null)
                    {
                        savedOptionValue = entityCreator();
                        savedOptionValue.Key = optionViewModel.OptionKey;
                    }
                    savedOptionValue.Value = optionViewModel.OptionValue;
                    savedOptionValue.Type = optionViewModel.Type;

                    ValidateOptionValue(savedOptionValue);

                    if (optionViewModel.Type == OptionType.Custom)
                    {
                        savedOptionValue.CustomOption = customOptions.First(co => co.Identifier == optionViewModel.CustomOption.Identifier);
                    }
                    else
                    {
                        savedOptionValue.CustomOption = null;
                    }

                    repository.Save(savedOptionValue);
                }
                else if (savedOptionValue != null)
                {
                    repository.Delete(savedOptionValue);
                }
            }
        }

        /// <summary>
        /// Saves the options - adds new ones and deleted the old ones..
        /// </summary>
        /// <typeparam name="TOption">The type of the option entity.</typeparam>
        /// <typeparam name="TEntity">The type of the option parent entity.</typeparam>
        /// <param name="optionContainer">The options container entity.</param>
        /// <param name="options">The list of new options.</param>
        public void SetOptions<TOption, TEntity>(IOptionContainer<TEntity> optionContainer, IEnumerable<IOption> options)
            where TEntity : IEntity
            where TOption : IDeletableOption<TEntity>, new()
        {
            var customOptions = LoadAndValidateCustomOptions(options);

            // Delete old ones
            if (optionContainer.Options != null)
            {
                foreach (var option in optionContainer.Options.Distinct())
                {
                    if (options == null || options.All(o => o.Key != option.Key))
                    {
                        if (!option.IsDeletable)
                        {
                            var message = string.Format(RootGlobalization.SaveOptions_CannotDeleteOption_Message, option.Key);
                            var logMessage = string.Format("Cannot delete option {0}, because it's marked as non-deletable.", option.Id);
                            throw new ValidationException(() => message, logMessage);
                        }

                        repository.Delete(option);
                    }
                }
            }

            // Add new / update existing
            if (options != null)
            {
                var optionsList = new List<IDeletableOption<TEntity>>();
                if (optionContainer.Options != null)
                {
                    optionsList.AddRange(optionContainer.Options);
                }

                foreach (var requestLayoutOption in options)
                {
                    TOption option = (TOption)optionsList.FirstOrDefault(o => o.Key == requestLayoutOption.Key);

                    if (option == null)
                    {
                        option = new TOption();
                        optionsList.Add(option);
                    }

                    option.Key = requestLayoutOption.Key;
                    option.Value = requestLayoutOption.Value;
                    option.Type = requestLayoutOption.Type;
                    option.Entity = (TEntity)optionContainer;

                    if (requestLayoutOption.Type == OptionType.Custom)
                    {
                        option.CustomOption = customOptions.First(co => co.Identifier == requestLayoutOption.CustomOption.Identifier);
                    }
                    else
                    {
                        option.CustomOption = null;
                    }

                    ValidateOptionValue(option);
                }

                optionContainer.Options = optionsList;
            }
        }

        /// <summary>
        /// Validates the option value.
        /// </summary>
        /// <param name="option">The option.</param>
        public void ValidateOptionValue(IOption option)
        {
            if (option != null && !string.IsNullOrWhiteSpace(option.Value))
            {
                try
                {
                    ConvertValueToCorrectType(option.Value, option.Type, option.CustomOption);
                }
                catch
                {
                    var message = string.Format(RootGlobalization.Option_Invalid_Message, option.Key, option.Type.ToGlobalizationString());

                    throw new ValidationException(() => message, message);
                }
            }
        }

        /// <summary>
        /// Validates the uniqueness of the option keys.
        /// </summary>
        /// <param name="options">The options.</param>
        public void ValidateOptionKeysUniqueness(IEnumerable<OptionViewModelBase> options)
        {
            var duplicateKey = options.GroupBy(option => option.OptionKey).Where(group => group.Count() > 1).Select(group => group.Key).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(duplicateKey))
            {
                var message = string.Format(RootGlobalization.Option_DuplicateKey_Message, duplicateKey);
                throw new ValidationException(() => message, message);
            }
        }

        /// <summary>
        /// Gets the safe value (doesn't fail on exception).
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>Value, converted to correct type or null, if conversion is impossible</returns>
        private object GetValueSafe(IOption option)
        {
            var value = option.Value;
            var type = option.Type;

            try
            {
                return ConvertValueToCorrectType(value, type, option.CustomOption);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts option value to the correct value type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="customOption">The custom option.</param>
        /// <returns>
        /// Value, converted to correct type
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        private object ConvertValueToCorrectType(string value, OptionType type, ICustomOption customOption)
        {
            if (string.IsNullOrEmpty(value))
            {
                return GetDefaultValueForType(type, customOption);
            }

            switch (type)
            {
                case OptionType.Text:
                    return value;

                case OptionType.Integer:
                    return Convert.ToInt64(value, CultureInfo.InvariantCulture);

                case OptionType.Float:
                    value = value.Replace(",", ".");
                    return Convert.ToDecimal(value, CultureInfo.InvariantCulture);

                case OptionType.DateTime:
                    return Convert.ToDateTime(value);

                case OptionType.Boolean:
                    return Convert.ToBoolean(value);

                case OptionType.Custom:
                    ICustomOptionProvider provider = GetCustomOptionsProvider(customOption);

                    if (provider != null)
                    {
                        return provider.ConvertValueToCorrectType(value);
                    }

                    return value;

                default:
                    throw new NotSupportedException(string.Format("Not supported option type: {0}", type));
            }
        }

        /// <summary>
        /// Gets the default value for specified option type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="customOption">The custom option.</param>
        /// <returns>
        /// Default types value
        /// </returns>
        private object GetDefaultValueForType(OptionType type, ICustomOption customOption)
        {
            switch (type)
            {
                case OptionType.Text:
                case OptionType.DateTime:
                case OptionType.Integer:
                case OptionType.Float:
                    return null;

                case OptionType.Boolean:
                    return false;

                case OptionType.Custom:
                    ICustomOptionProvider provider = GetCustomOptionsProvider(customOption);

                    if (provider != null)
                    {
                        return provider.GetDefaultValueForType();
                    }

                    return null;

                default:
                    throw new NotSupportedException(string.Format("Not supported option type: {0}", type));
            }
        }

        /// <summary>
        /// Loads and validate custom options.
        /// </summary>
        /// <returns>The list of custom option entities</returns>
        private IList<CustomOption> LoadAndValidateCustomOptions(IEnumerable<IOption> options)
        {
            // Check if options are valid
            var invalidOption = options.FirstOrDefault(o => o.Type == OptionType.Custom && string.IsNullOrWhiteSpace(o.CustomOption.Identifier));
            if (invalidOption != null)
            {
                throw new InvalidOperationException(string.Format("Custom option type provider must be set for custom type! Option Key: {0}", invalidOption.Key));
            }

            // Get already loaded custom options or option types
            List<CustomOption> customOptions = options
                .Where(o => o.Type == OptionType.Custom && o.CustomOption is CustomOption && !(o.CustomOption is IProxy))
                .Select(o => (CustomOption)o.CustomOption)
                .ToList();

            // Load missing custom options
            var customOptionsIdentifiers = options
                .Where(o => o.Type == OptionType.Custom 
                    && !(o.CustomOption is CustomOption) 
                    && customOptions.All(co => co.Identifier != o.CustomOption.Identifier))
                .Select(o => o.CustomOption.Identifier)
                .Distinct().ToArray();

            if (customOptionsIdentifiers.Length == 0)
            {
                return customOptions;
            }

            customOptions.AddRange(GetCustomOptionsById(customOptionsIdentifiers));

            return customOptions;
        }

        /// <summary>
        /// Loads the custom option entities by specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>
        /// List of custom option entities
        /// </returns>
        public List<CustomOption> GetCustomOptionsById(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return new List<CustomOption>();
            }

            var customOptions = repository
                .AsQueryable<CustomOption>()
                .Where(co => ids.Contains(co.Identifier))
                .ToList();

            // Validate if there are any missing custom options
            var notExisting = ids.FirstOrDefault(i => customOptions.All(co => co.Identifier != i));
            if (notExisting != null)
            {
                throw new InvalidOperationException(string.Format("Custom option provider not found for custom type {0}!", notExisting));
            }

            return customOptions;
        }

        /// <summary>
        /// Gets the custom options provider.
        /// </summary>
        /// <returns>Custom options provider</returns>
        private ICustomOptionProvider GetCustomOptionsProvider(ICustomOption customOption)
        {
            if (customOption == null)
            {
                return null;
            }

            return CustomOptionsProvider.GetProvider(customOption.Identifier);
        }
    }
}