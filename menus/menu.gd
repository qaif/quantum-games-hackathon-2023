extends Node2D


func _on_quit_button_pressed():
	get_tree().quit()


func _on_play_button_pressed():
	if(GlobalVar.level_selected == 1):
		get_tree().change_scene_to_file("res://scenes/level1.tscn")


func _on_option_button_pressed():
	get_tree().change_scene_to_file("res://menus/choose_level.tscn")
