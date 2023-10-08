extends Node2D

var temperature
var in_shower = false

var min_temperature = -273.15
var max_temperature = -263

var scale_min = 2.138
var scale_max =  6.32

# Fonction appelée au démarrage de la scène
func _ready():
	temperature = min_temperature

# Fonction pour augmenter la taille du rectangle vers le haut
func increase_degree(degree : float):
	# Augmentez la hauteur du rectangle
	if !in_shower:
		if temperature < max_temperature:
			temperature += degree
			$degree.scale.y = (scale_max - scale_min) * (temperature - min_temperature)/(max_temperature-min_temperature) + scale_min
		else : 
			get_tree().change_scene_to_file("res://menus/lose_menu.tscn")
	
func decrease_degree(degree : float):	
	if temperature > min_temperature :
		temperature -= degree
		$degree.scale.y = (scale_max - scale_min) * (temperature - min_temperature)/(max_temperature-min_temperature) + scale_min


func get_temperature():
	return temperature


func _on_timer_timeout():
	increase_degree(0.1)
		
		

	
	

