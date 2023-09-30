extends Node2D




# Fonction appelée au démarrage de la scène
func _ready():
	pass # Initialisez les nœuds du thermomètre

# Fonction pour augmenter la taille du rectangle vers le haut
func increase_degree(degree : int):
	# Augmentez la hauteur du rectangle
	$degree.scale.y += degree
	
func decrease_degree(degree : int):
	# Augmentez la hauteur du rectangle
	$degree.scale.y -= degree

func _on_button_pressed():
	increase_degree(1)
