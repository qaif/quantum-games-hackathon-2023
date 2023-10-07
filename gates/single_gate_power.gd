@tool
extends Node2D

@export var letter = "H"
@export var text_color : Color = Color(1, 1, 1, 1)
@export var bg_color : Color = Color(0, 0, 0, 1)

# Called when the node enters the scene tree for the first time.
func _ready():
	$Panel/Label.text = letter
	$Panel/Label.add_theme_color_override("font_color", text_color)
	$Panel.modulate = bg_color

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	$Panel/Label.text = letter
	$Panel/Label.add_theme_color_override("font_color", text_color)
	$Panel.modulate = bg_color

	


func _on_area_2d_body_entered(body):
	var inventory = get_node("/root/inv")
	if letter == "H":
		inventory.hadamard += 1
	if letter == "X":
		inventory.paulix += 1
	if letter == "Y":
		inventory.pauliy += 1
	if letter == "Z":
		inventory.pauliz += 1
	$"../Player".change_state(letter)
	queue_free()
