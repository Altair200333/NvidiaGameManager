using ReactiveUI;
using static NvidiaGameManager.Views.MainView;
using System;

namespace NvidiaGameManager.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string[] AllDisplays { get; set;  } = new string[]{};
    public string[] AllConfigs { get; set;  } = new string[]{};
}
