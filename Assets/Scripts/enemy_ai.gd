extends Node2D

var enemy: CharacterBody2D
@export var speed := 90

var state := "alive" # "alive", "knocked", "attached"

func _ready() -> void:
	enemy = get_parent()

func _physics_process(_delta: float) -> void:
	if state != "alive":
		return
	# very simple chase AI
	var player := get_tree().get_root().get_node_or_null("GameScene/Player")
	if player:
		var dir = (player.global_position - global_position).normalized()
		enemy.velocity = dir * speed
		enemy.move_and_slide()
