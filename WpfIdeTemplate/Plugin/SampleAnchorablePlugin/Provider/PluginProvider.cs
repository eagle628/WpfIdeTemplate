﻿using Microsoft.Extensions.Logging;
using SampleCompany.SampleProduct.PluginUtility;
using SampleCompany.SampleProduct.SampleAnchorablePlugin.ViewModel;

namespace SampleCompany.SampleProduct.SampleAnchorablePlugin.Provider
{
    public class PluginProvider : IPluginProvider
    {

        public object CreatePluginObject(IAppServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<SampleAnchorableViewModel>>();
            return new SampleAnchorableViewModel(logger);
        }

        public PluginProvider()
        {

        }
    }
}
