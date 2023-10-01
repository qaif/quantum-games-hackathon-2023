extends Node2D

var temperature


# Fonction appelée au démarrage de la scène
func _ready():
	temperature = -269

# Fonction pour augmenter la taille du rectangle vers le haut
func increase_degree(degree : float):
	# Augmentez la hauteur du rectangle
	if temperature < -263 :
		temperature += degree
		$degree.scale.y += 0.07
	else : 
		get_tree().change_scene_to_file("res://lose_menu.tscn")
func decrease_degree(degree : float):	
	if temperature > -269 :
		$degree.scale.y -= 0.07
		temperature -= degree


	
func get_temperature():
	return temperature


func _on_timer_timeout():
	increase_degree(0.1)
	$Label.text = str("%.2f"%temperature) + "°C"
