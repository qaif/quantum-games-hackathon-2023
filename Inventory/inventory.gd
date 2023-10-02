extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var inventory = get_node("/root/inv")
	$H_label.text = str(inventory.hadamard) + " / 5"
	$X_label.text = str(inventory.paulix) + " / 5"
	$Y_label.text = str(inventory.pauliy) + " / 5"
	$Z_label.text = str(inventory.pauliz) + " / 5"
