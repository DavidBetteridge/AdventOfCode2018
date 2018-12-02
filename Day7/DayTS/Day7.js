"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var fs = require("fs"); //npm install --save-dev @types/node
var Dependencies = /** @class */ (function () {
    function Dependencies() {
        this.SearchFor = {};
        this.Count = 0;
    }
    return Dependencies;
}());
exports.Dependencies = Dependencies;
var ElfWorker = /** @class */ (function () {
    function ElfWorker() {
    }
    return ElfWorker;
}());
var Step = /** @class */ (function () {
    function Step(line) {
        line = line.replace(" must be finished before step ", "");
        line = line.replace(" can begin.", "");
        line = line.replace("Step ", "");
        this.fromStep = line[0];
        this.toStep = line[1];
    }
    return Step;
}());
var Loader = /** @class */ (function () {
    function Loader() {
    }
    Loader.prototype.Load = function () {
        var file = fs.readFileSync('Steps.txt', 'utf8');
        file = file.replace(/^\uFEFF/, '');
        var steps = file.split("\r\n");
        var dependencies = new Dependencies();
        var allSteps = new Array(0);
        steps.forEach(function (line) {
            var step = new Step(line);
            if (allSteps.indexOf(step.fromStep) < 0)
                allSteps.push(step.fromStep);
            var dependsOn = dependencies.SearchFor[step.toStep];
            if (typeof dependsOn === "undefined") {
                dependsOn = new Array(0);
                dependencies.SearchFor[step.toStep] = dependsOn;
                dependencies.Count++;
            }
            dependsOn.push(step.fromStep);
            var fromStep = dependencies.SearchFor[step.fromStep];
            if (typeof fromStep === "undefined") {
                dependencies.SearchFor[step.fromStep] = new Array(0);
                dependencies.Count++;
            }
        });
        return dependencies;
    };
    return Loader;
}());
var Startup = /** @class */ (function () {
    function Startup() {
    }
    Startup.main = function () {
        var loader = new Loader();
        var dependencies = loader.Load();
        var answer = "";
        while (dependencies.Count > 0) {
            var possible = new Array(0);
            Object.keys(dependencies.SearchFor).forEach(function (d) {
                var dependsOn = dependencies.SearchFor[d];
                if (dependsOn.length == 0) {
                    possible.push(d);
                }
            });
            var next = possible.sort(function (one, two) { return (one > two ? 1 : -1); })[0];
            Object.keys(dependencies.SearchFor).forEach(function (d) {
                var dependsOn = dependencies.SearchFor[d];
                var index = dependsOn.indexOf(next, 0);
                if (index > -1) {
                    dependsOn.splice(index, 1);
                    dependencies.SearchFor[d] = dependsOn;
                }
            });
            delete dependencies.SearchFor[next];
            dependencies.Count--;
            answer += next;
        }
        console.log(answer);
        return 0;
    };
    return Startup;
}());
Startup.main();
//# sourceMappingURL=Day7.js.map