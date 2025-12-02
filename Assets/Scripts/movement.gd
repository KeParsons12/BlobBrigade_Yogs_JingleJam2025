extends Node2D

# @export is like [serializefield] by allowing the variable to be in the inspector
@export var move_speed := 200

# players input
var input_dir := Vector2.ZERO

# Cache a reference to the parent Player ( must be CharacterBody2D)
var player: CharacterBody2D

func _ready() -> void:
	player = get_parent() # The actual body that moves

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	input_dir = get_move_input_vector()
	look_at_mouse()

func _physics_process(_delta: float) -> void:
	movement()

# Rotates the node to look at the mouse position
func look_at_mouse() -> void:
	var mouse_pos = get_global_mouse_position()
	player.look_at(mouse_pos)

func get_move_input_vector() -> Vector2:
	return Input.get_vector("move_left", "move_right", "move_up", "move_down")
	#return Vector2(
		#Input.get_axis("move_left", "move_right"), 
		#Input.get_axis("move_up", "move_down")
	#)

func movement() -> void:
	player.velocity = input_dir.normalized() * move_speed
	player.move_and_slide()
