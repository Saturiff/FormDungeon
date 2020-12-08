### Todo:
鼠標點擊Viewport對該方向開火，由不同種類的武器決定開火方式

Client to server:
fire ->
code|player_name|is_fire?|direction
hit ->
code|to_player|damage

Client:
fire:
draw by weapon type
projection = rect(a, b)
hit

傳遞開火與停火訊號至服務器
玩家名稱 + 開火
玩家名稱 + 停火
製作武器Dist<>
角色死亡後等待數秒，隨機地點重生
health bar
死亡時噴裝
重生時滿血
畫面震動

test sv side health set { } area
玩家移動速度
