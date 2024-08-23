extends GdUnitTestSuite


# Called when the node enters the scene tree for the first time.
func test_audio_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/AudioManager/Scenes/AudioTest/AudioTest.tscn")

	#Simulate for 5 seconds = 300 frames
    await runner.simulate_frames(300)

func test_FSM_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/FSM/Scenes/FSMTest/FSMTest.tscn")

	#Simulate for 5 seconds = 300 frames
    await runner.simulate_frames(300)

func test_input_remapper_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/Input/InputRemapper/Scene/InputRemapperTest.tscn")

	#Simulate for 5 seconds = 300 frames
    await runner.simulate_frames(300)


func test_log_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/LogManager/Scenes/LogTest/LogTest.tscn")

	#Simulate for 5 seconds = 300 frames
    await runner.simulate_frames(300)

func test_particle_system_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/ParticleSystemController/Scenes/ParticleSystemController/ParticleSystemController.tscn")

	#Simulate for 5 seconds = 300 frames
    await runner.simulate_frames(300)


func test_scene_transition_test_scene(timeout = 20000) -> void:
    print("running test with timeout " + str(timeout))
    # Create the scene runner for scene `test_scene.tscn`
    var runner := scene_runner("res://CoreTools/SceneTransitionManager/Scene/SceneOne/ExampleScene1.tscn")

	#Simulate for 5 seconds = 300 frames
    await runner.simulate_frames(300)
