done = ->
	console.log "**Connected** to the CommitService event stream"

fail = ->
	console.log "Could not establish connection to the CommitService event stream"

error = ->
	console.log "error communicating with the CommitService event stream"

listenForEvents = ->
	$.connection.hub.start({ jsonp: true }).done(done).fail(fail)

	$.connection.hub.error(error)

	$.connection.eventStream.client.Notify = (topic, message) ->		
		data = JSON.parse message
		newItem = $('#commit-template').tmpl([data])
		newItem.prependTo("#commits-list")
		newItem.fadeOut(100)
		newItem.fadeIn(1500)
		newItem.css({border:"2px solid darkgreen"})
		        
window.listenForEvents = listenForEvents