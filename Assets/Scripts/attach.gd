extends Node2D

# limit the amount of objects that can attach
@export var max_attached := 5
var attached_objects := []

func attach_object(obj: Node) -> void:
	var attachable_node = obj.get_node_or_null("Attachable")
	if not attachable_node: # if the object doesnt have the node
		return
	if not attachable_node.attachable: # if the object has the attachable node but is not attachable
		return
	if attached_objects.size() >= max_attached: # if too many objects are attached already
		return
	
	# store old global position
	var old_global = obj.global_position
	
	# parent the whole object to the player
	get_parent().add_child(obj)
	
	# keep its position
	obj.global_position = old_global
	
	# mark as attached
	attachable_node.attachable = false
	attached_objects.append(obj)

func detach_object(obj: Node) -> void:
	if obj in attached_objects:
		# unparent back to the scene root (or world container)
		remove_child(obj)
		get_tree().get_current_scene().add_child(obj)
		
		# erase the object from list
		attached_objects.erase(obj)
		var attachable_node = obj.get_node_or_null("Attachable")
		if attachable_node:
			attachable_node.attachable = true
