define([
    "dojo/_base/declare",
    "epi/_Module",
    "epi/routes"
], function (
    declare,
    _Module,
    routes
) {
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);

            var registry = this.resolveDependency("epi.storeregistry");
            // remove existing facet store
            if (registry.get("epi.commerce.facet")) {
                delete registry._stores["epi.commerce.facet"];
            }
            // register the custom facet
            registry.create("epi.commerce.facet", routes.getRestPath({ moduleArea: "app", storeName: "customfacet" }));
        }
    });
});