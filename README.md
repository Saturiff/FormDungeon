# Form dungeon - 表單地下城
期末作業，編碼照著自己常用的規範撰寫，部分流程是刻意為了練習C#的功能而寫出，供未來查考。  

## Warning!
在IDE中的除錯模式下，若客戶端因出錯而崩潰或擲出任何例外狀況導致非正常下線，該名玩家無法再次正常連線，需要開發者重新開啟一個新的服務器實體才能正常除錯。  


### 檔案樹
Client  
. /  
├─Maps  
│ └─Map.csv  
├─DungeonGame.exe  
└─DungeonUtility.dll  
  
Server  
. /  
├─Maps  
│ └─Map.csv  
├─Saves  
│ └─<Saved player data, generated in runtime>  
├─DungeonServer.exe  
└─DungeonUtility.dll  
  