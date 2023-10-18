extends Node2D

var circuitOrder = ["X","H","H", "CNOT", "CNOT", "CNOT", "H","H"]
var circuitIndex = 0;

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func nextFrame(gate):
	if (gate == circuitOrder[circuitIndex]): 
		circuitIndex += 1
		$AnimatedSprite2D.frame = circuitIndex
	else: 
		get_tree().change_scene_to_file("res://menus/short_circuit.tscn")
	if circuitIndex == circuitOrder.size(): 
		get_tree().change_scene_to_file("res://menus/winning_screen.tscn")
