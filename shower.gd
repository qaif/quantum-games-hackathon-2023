extends Node2D

var thermometer_reference
var shower
# Called when the node enters the scene tree for the first time.
func _ready():
	shower = false


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass



func _on_area_2d_body_entered(body):
	print("entered with shower")
	shower = true

	
func _on_area_2d_body_exited(body):
	print("exited with shower")
	shower = false
	get_node("../CanvasLayer/thermometer").in_shower = false


func _on_timer_timeout():
		if shower:
			#print("shower active")
			get_node("../CanvasLayer/thermometer").in_shower = true
			get_node("../CanvasLayer/thermometer").decrease_degree(0.10)
