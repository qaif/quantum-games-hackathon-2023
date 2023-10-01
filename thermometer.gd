extends Node2D

var temperature
var in_shower = false


# Fonction appelée au démarrage de la scène
func _ready():
	temperature = -269

# Fonction pour augmenter la taille du rectangle vers le haut
func increase_degree(degree : float):
	# Augmentez la hauteur du rectangle
	if temperature < -263 :
		temperature += degree
		$degree.scale.y += 0.07
		$Label.text = str("%.2f"%temperature) + "°C"
	else : 
		get_tree().change_scene_to_file("res://lose_menu.tscn")
	
func decrease_degree(degree : float):	
	if temperature > -269 :
		$degree.scale.y -= 0.07
		temperature -= degree
		$Label.text = str("%.2f"%temperature) + "°C"

func get_temperature():
	return temperature


func _on_timer_timeout():
	if !in_shower:
		increase_degree(0.1)
		$Label.text = str("%.2f"%temperature) + "°C"
		

	
	

