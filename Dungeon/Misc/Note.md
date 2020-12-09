### Todo:
Client to server:
fire
code|player_name|is_fire?|direction
!> hit
!> code|to_player|damage

Client:
fire:
hit

傳遞開火與停火訊號至服務器
玩家名稱 + 開火
玩家名稱 + 停火
角色死亡後等待數秒，隨機地點重生
health bar
死亡時噴裝
重生時滿血
畫面震動

test sv side health set { } area

服務器防止生成物品重疊
客戶端將所有可撿起物件移至最底層

client fire pending
server fire 

client hit ok
server hit pending
