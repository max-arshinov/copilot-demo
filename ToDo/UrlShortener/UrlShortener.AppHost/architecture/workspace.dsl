workspace "Url Shortener" {
    !identifiers hierarchical
    !constant WEB_LANG "C#/.NET 9"
    !script groovy {
      workspace.model.impliedRelationshipsStrategy = new com.structurizr.model.CreateImpliedRelationshipsUnlessSameRelationshipExistsStrategy()
    }
    properties {
        "structurizr.groupSeparator" "/"
    }

    model {
    }
    
    views {
        theme default
    }
    
    !script groovy {
        workspace.views.createDefaultViews()
        workspace.views.views.findAll { it instanceof com.structurizr.view.ModelView }.each { it.disableAutomaticLayout() }
    }
}