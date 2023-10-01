@tool
extends Node2D

@export var letter = "H"
@export var text_color : Color = Color(1, 1, 1, 1)
@export var bg_color : Color = Color(0, 0, 0, 1)

# Called when the node enters the scene tree for the first time.
func _ready():
	$Panel/Label.text = letter

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	$Panel/Label.text = letter
	$Panel/Label.modulate = text_color

	
