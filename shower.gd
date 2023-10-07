extends Node2D

var thermometer_reference
var shower = false
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	shower = false



func _on_area_2d_body_entered(body):
	shower = true

	
func _on_area_2d_body_exited(body):
	shower = false
	get_node("../CanvasLayer/thermometer").in_shower = false


func _on_timer_timeout():
		if shower:
			get_node("../CanvasLayer/thermometer").in_shower = true
			get_node("../CanvasLayer/thermometer").decrease_degree(0.10)
