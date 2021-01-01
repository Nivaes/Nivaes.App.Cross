// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Core.Parse.StringDictionary
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.Base;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.ViewModels;

    public class MvxViewModelRequestCustomTextSerializer
        : IMvxTextSerializer
    {
        private IMvxViewModelByNameLookup? mByNameLookup;

        protected IMvxViewModelByNameLookup ByNameLookup
        {
            get
            {
                mByNameLookup ??= Mvx.IoCProvider.Resolve<IMvxViewModelByNameLookup>();
                return mByNameLookup;
            }
        }

        public T DeserializeObject<T>(string inputText)
        {
            return (T)DeserializeObject(typeof(T), inputText);
        }

        public string SerializeObject(object toSerialise)
        {
            if (toSerialise is MvxViewModelRequest mvxViewModelRequest)
                return Serialize(mvxViewModelRequest);

            if (toSerialise is IDictionary<string, string> dictionarySerilise)
                return Serialize(dictionarySerilise);

            throw new MvxException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
        }

        public object DeserializeObject(Type type, string inputText)
        {
            if (type == typeof(MvxViewModelRequest))
                return DeserializeViewModelRequest(inputText);

            if (typeof(IDictionary<string, string>).IsAssignableFrom(type))
                return DeserializeStringDictionary(inputText);

            throw new MvxException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
        }

        protected virtual IDictionary<string, string> DeserializeStringDictionary(string inputText)
        {
            var stringDictionaryParser = new MvxStringDictionaryParser();
            var dictionary = stringDictionaryParser.Parse(inputText);
            return dictionary;
        }

        protected virtual MvxViewModelRequest DeserializeViewModelRequest(string inputText)
        {
            var stringDictionaryParser = new MvxStringDictionaryParser();
            var dictionary = stringDictionaryParser.Parse(inputText);
            var toReturn = new MvxViewModelRequest();
            var viewModelTypeName = SafeGetValue(dictionary, "Type");
            toReturn.ViewModelType = DeserializeViewModelType(viewModelTypeName);
            toReturn.ParameterValues = stringDictionaryParser.Parse(SafeGetValue(dictionary, "Params"));
            toReturn.PresentationValues = stringDictionaryParser.Parse(SafeGetValue(dictionary, "Pres"));
            return toReturn;
        }

        protected virtual string Serialize(IDictionary<string, string> toSerialise)
        {
            var stringDictionaryWriter = new MvxStringDictionaryWriter();
            return stringDictionaryWriter.Write(toSerialise);
        }

        protected virtual string Serialize(MvxViewModelRequest toSerialise)
        {
            if (toSerialise == null)
                throw new NullReferenceException(nameof(toSerialise));

            var stringDictionaryWriter = new MvxStringDictionaryWriter();

            var dictionary = new Dictionary<string, string>
            {
                ["Type"] = SerializeViewModelName(toSerialise.ViewModelType),
                ["Params"] = stringDictionaryWriter.Write(toSerialise.ParameterValues),
                ["Pres"] = stringDictionaryWriter.Write(toSerialise.PresentationValues)
            };

            return stringDictionaryWriter.Write(dictionary);
        }

        protected virtual string SerializeViewModelName(Type viewModelType)
        {
            if (viewModelType == null)
                throw new NullReferenceException(nameof(viewModelType));

            return viewModelType.FullName;
        }

        protected virtual Type DeserializeViewModelType(string viewModelTypeName)
        {
            if (!ByNameLookup.TryLookupByFullName(viewModelTypeName, out Type toReturn))
                throw new MvxException("Failed to find viewmodel for {0} - is the ViewModel in the same Assembly as App.cs? If not, you can add it by overriding GetViewModelAssemblies() in setup", viewModelTypeName);

            return toReturn;
        }

        private string SafeGetValue(IDictionary<string, string> dictionary, string key)
        {
            if (!dictionary.TryGetValue(key, out string value))
                throw new MvxException("Dictionary missing required keyvalue pair for key {0}", key);
            return value;
        }
    }
}
