<br/>
<br/>
<div align="center">
    <img src="Release/Utility/Clock_CSharp.widget/images/clock.PNG" style="zoom:80%"/>
</div>
<br/>
<br/>

# Contents
- [English](#Description)
- [繁體中文](#描述)

# **Description**
This is a plug-in that can display clock faces; you can choose the city time zone and even switch between a digital or an analog clock face.

# **Features**
* Code in Visual Studio C#
* Drop-down menu to select the city time you want and change the clock type (digital or analog)
* Tap the screen to change the clock type

# **Development Environment**
Comptabile to Win OS 10 and above
Developed with Visual Studio 2019

# **Overview**
There are three main roles in this application.
1. Creator Central
2. Widget(Controller)
3. Property
This application demo shows how to implement a clock widget using C#.

When Creator Central starts Widget (Controller), Creator Central will send two parameters to Widget (Controller), namely Widget UUID and port. The two communicate through WebSocket. The follow-up commands need to include the Widget UUID information for identification, and the relevant definitions such as the packet format are explained in [The Overview of Creator Central SDK](https://github.com/AVerMedia-Technologies-Inc/CreatorCentralSDK).
The clock is created using WinForm, and the content drawn on View is converted into a picture, and then converted into a Base64 String and sent to Creator Central.

Adjust the time zone and clock type.
|  key   | value  |
|  ----  | ----  |
| city  | Taipei / New_York / California |
| type  | analog / digital |

The current demo provides three cities and two clock types to choose from. You can modify the time zone or clock type by using Property, and then send the parameters to Widget (Controller). Widget (Controller) draws a new screen based on the received parameters.
It is displayed in Creator Central, and at the same time, the configuration file of Creator Central is also updated to ensure that it will be in the last operation state when the application is opened next time.

You can see the complete [Time Zone List](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones)

# **Application File Description**
|  File Name  |  Content Description
|  ----  | ----  |
| [WebSocketSharp.1.0.3-rc11](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/tree/main/Sources/ClockWidget_CSharp/packages/WebSocketSharp.1.0.3-rc11)  | WebSocket Library for C# |
| [newtonsoft.json.13.0.1](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/tree/main/Sources/ClockWidget_CSharp/packages/newtonsoft.json.13.0.1)  | .json library for C# |
| [Blank.png](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/blob/main/Sources/ClockWidget_CSharp/Resources/Blank.png)  | Background image for digital clock |
| [ClockJPG.jpg](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/blob/main/Sources/ClockWidget_CSharp/Resources/ClockJPG.jpg)  | Background image for analog clock |
| [PluginForm.cs](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/blob/main/Sources/ClockWidget_CSharp/PluginForm.cs)  | main flow program and main form |
| [Websocket2Client.cs](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/blob/main/Sources/ClockWidget_CSharp/Websocket2Client.cs)  | websocket connection class |

Note: Creator Central only supports String data type. If a Data type packet is sent, Creator Central will not take any action.

# **Installation**
Integrate the compiled executable file into the Widget directory (Clock_CSharp.widget/).
Place the Widget directory to "C:/Users/＜YourName＞/AppData/Roaming/AVerMedia Creator Central/widgets/＜PackageName＞/", then open the Creator Central machine to see it.
achine to see it.

# **Uninstallation**
Close Creator Central, and go to "C:/Users/＜YourName＞/AppData/Roaming/AVerMedia Creator Central/widgets/＜PackageName＞/", delete the Clock_CSharp.widget folder, and then open Creator Central again.

# **Troubleshooting for running failure**
1. Make sure the Widget directory is placed under folder "C:/Users/＜YourName＞/AppData/Roaming/AVerMedia Creator Central/widgets/＜PackageName＞/".
2. In the WidgetConfig.json file in the Widget directory, check whether the Win Target name is the name of the executable file: ClockWidget_CSharp.exe.

- - -


# **描述**
這是一個可以顯示時鐘的插件，你可以選擇想要的城市時區，甚至還可以變換數位時鐘或是類比時鐘

# **特徵**
* 用 Visual Studio C# 編寫程式碼
* 下拉式選單選擇你想要的城市時間及改變時鐘的類型（數位或是類比）
* 點擊螢幕即可更換時鐘類型

# **開發環境**
適用於 Win OS 10 以上版本
使用 Visual Studio 2019 版本開發

# **整體概要說明**
整個應用主要有三個角色
1. Creator Central
2. Widget(Controller)
3. Property
而本範例程式展示的是使用 C# 如何實作一個時鐘的 Widget

當 Creator Central 啟動 Widget（Controller） 時， Creator Central 會派發兩個參數給 Widget（Controller） ，分別是 Widget UUID 以及 port。兩者間透過 WebSocket 進行溝通。後續的指令溝通都需要包含 Widget UUID 這資訊用以識別，封包格式等相關定義在 [The Overview of Creator Central SDK](https://github.com/AVerMedia-Technologies-Inc/CreatorCentralSDK) 有更進一步的說明。

時鐘的製作主要是使用 C# 繪製，將繪製在 View 上的內容轉換成圖片，接著再轉換成 Base64 String 傳送至 Creator Central 。

調整時區和時鐘的類型
|  key   | value  |
|  ----  | ----  |
| city  | taipei / new_york / california |
| type  | analog / digital |

目前範例程式中提供了三個城市和兩種時鐘類型做選擇。
透過操作 Property 修改時區或時鐘的類型，然後將參數傳送至 Widget（Controller）。 Widget（Controller） 根據收到的參數繪製新的畫面在 Creator Central 顯示出來。與此同時也一併更新 Creator Central 的設定檔，確保下次開啟應用程式時，會是上次最後的操作記錄。


在這個網址可以看到所有的 [時區列表](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones)

# **程式檔案說明**
|  檔案名稱  | 內容描述  |
|  ----  | ----  |
| [WebSocketSharp.1.0.3-rc11](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/tree/main/Sources/ClockWidget_CSharp/packages/WebSocketSharp.1.0.3-rc11)  | WebSocket Library for C# |
| [newtonsoft.json.13.0.1](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/tree/main/Sources/ClockWidget_CSharp/packages/newtonsoft.json.13.0.1)  | .json library for C# |
| [Blank.png](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/blob/main/Sources/ClockWidget_CSharp/Resources/Blank.png)  | Background image for digital clock |
| [ClockJPG.jpg](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/blob/main/Sources/ClockWidget_CSharp/Resources/ClockJPG.jpg)  | Background image for analog clock |
| [PluginForm.cs](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/blob/main/Sources/ClockWidget_CSharp/PluginForm.cs)  | main flow program and main form |
| [Websocket2Client.cs](https://github.com/AVerMedia-Technologies-Inc/WidgetCShapeSample/blob/main/Sources/ClockWidget_CSharp/Websocket2Client.cs)  | websocket connection class |


注意：Creator Central 只支援 String 資料型別. 如果發送 Data 資料型別的封包的話， Creator Central 將不會有任何動作。

# **安裝**
將編譯後執行檔整合進 Widget 目錄 （Clock_CSharp.widget/)
將 Widget 目錄放置到 "C:/Users/＜YourName＞/AppData/Roaming/AVerMedia Creator Central/widgets/＜PackageName＞/" ，接著打開 Creator Central 機器即可看到。

# **反安裝**
關掉 Creator Central ，到 "C:/Users/＜YourName＞/AppData/Roaming/AVerMedia Creator Central/widgets/＜PackageName＞/" 目錄下把 Clock_CSharp.widget 資料夾刪掉，再次打開 Creator Central 即可。

# **無法執行時的問題排除措施說明**
1. 確認 Widget 目錄是否有確實放置到 "C:/Users/＜YourName＞/AppData/Roaming/AVerMedia Creator Central/widgets/＜PackageName＞/" 資料夾之下
2. 檢查 Widget 目錄裡的 WidgetConfig.json 檔案中， Win 的 Target 名稱是否是執行檔的名稱 （ClockWidget_CSharp.exe）
