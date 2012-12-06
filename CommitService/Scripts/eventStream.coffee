class EventStream
	constructor: (@hubName="eventStream", @autoStart=true, @debug=true) ->
		@_topics = []
		@_clientDefine "Receive", @receive
		if @autoStart
			@start()

	start: =>
		console.log "Starting connection" if @debug
		# NOTE: { jsonp: true } causes groups (for topics) to fail...
		$.signalR.hub.start({  }).done(@done).fail(@fail)
		$.signalR.hub.error @error

	subscribe: (topic, callback) =>
		@[topic] = callback
		@_topics.push topic

	on: (topic, callback) =>
		@subscribe topic, callback

	receive: (topic, message) =>
		try
			console.log "Receive: " + topic if @debug
			message = JSON.parse(message)
			@[topic] message
		catch err
			console.log err if @debug
			@error err

	done: =>
		console.log "Connected successfully to the EventStream" if @debug
		for topic in @_topics
			@_serverInvoke 'subscribe', topic

	fail: =>
		console.log "Could not establish connection to the EventStream" if @debug

	error: (error) ->
		console.log("Error communicating with the EventStream: " + error) if @debug

	_clientDefine: (memberName, func) ->
		$.signalR[@hubName].client[memberName] = func

	_serverInvoke: (funcName, arg) ->
		$.signalR[@hubName].server[funcName] arg

window.EventStream = EventStream

"""
class LiveList extends EventStream
	constructor: (targetSelector, formatFunc, @hubName="eventStream", @autoStart=true, @debug=true) ->
		super(@hubName, @autoStart, @debug)
"""