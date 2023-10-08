extends Node2D

var circuitOrder = ["H","CNOT","CNOT","H","Z","X"]
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
		get_tree().change_scene_to_file("res://menus/lose_menu.tscn")
	if circuitIndex == circuitOrder.size(): 
		get_tree().change_scene_to_file("res://menus/winning_screen.tscn")
