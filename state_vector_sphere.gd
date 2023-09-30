extends Node2D

var DIR = OS.get_executable_path().get_base_dir()
var interpreter_path = DIR.plus_file("PythonFiles/venv/Scripts/activate")
var script_path = DIR.plus_file("PythonFiles/bridging.py")

var temperature = 0;

# Called when the node enters the scene tree for the first time.
func _ready():
	if !OS.has_feature("standalone"):
		interpreter_path = ProjectSettings.globalize_path("res://PythonFiles/venv/Scripts/activate")
		script_path = ProjectSettings.globalize_path("PythonFiles/venv/Scripts/activate")
	var gate_type= ""
	compute_state_vector(gate_type, 2, 3)

func compute_state_vector(a,b,c):
	pass
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
		
func increase_temperature(degree):
	temperature += degree
	
func decrease_temperature(degree):
	temperature -= degree 
