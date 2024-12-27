using System;
using System.Collections.Generic;
using UnityEngine;

namespace Src.Registry
{
    [Serializable]
    public class NameToVersion
    {
        public string name;
        public string version;
    }
    
    [CreateAssetMenu(fileName = "ServicesRegistryConfig", menuName = "Configs/ServicesRegistryConfig", order = 0)]
    public class ServicesRegistryConfig : ScriptableObject
    {
        [SerializeField] string registryUrl;
        [SerializeField] List<NameToVersion> authServices;
        [SerializeField] List<NameToVersion> schemasServices;
        [SerializeField] List<NameToVersion> solutionsServices;
        
        public string RegistryUrl => registryUrl;
        public IReadOnlyCollection<NameToVersion> AuthServices => authServices;
        public IReadOnlyCollection<NameToVersion> SchemasServices => schemasServices;
        public IReadOnlyCollection<NameToVersion> SolutionsServices => solutionsServices;
    }
}