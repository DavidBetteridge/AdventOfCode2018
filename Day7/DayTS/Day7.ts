import fs = require('fs');  //npm install --save-dev @types/node

interface Dictionary<T> {
    [Key: string]: T;
}

export class Dependencies {
    SearchFor: Dictionary<string[]> = {};
    Count: number = 0;
}


class ElfWorker {
    currentActivity: string;
    timeRemaining: number;
}

class Step {
    fromStep: string;
    toStep: string;

    constructor(line: string) {
        line = line.replace(" must be finished before step ", "");
        line = line.replace(" can begin.", "");
        line = line.replace("Step ", "");

        this.fromStep = line[0];
        this.toStep  = line[1];
    }
}

class Loader {

    public Load(): Dependencies {
        var file = fs.readFileSync('Steps.txt','utf8');
        file = file.replace(/^\uFEFF/, '');
        var steps = file.split("\r\n");

        var dependencies = new Dependencies();
        var allSteps:string[] = new Array(0);

        steps.forEach(line => {
            var step = new Step(line);

            if (allSteps.indexOf(step.fromStep) < 0)
               allSteps.push(step.fromStep);

            var dependsOn = dependencies.SearchFor[step.toStep];             
            if (typeof dependsOn === "undefined")
            {
                dependsOn = new Array(0);
                dependencies.SearchFor[step.toStep] = dependsOn;                   
                dependencies.Count++;
            }
            dependsOn.push(step.fromStep);    
   
            var fromStep = dependencies.SearchFor[step.fromStep];             
            if (typeof fromStep === "undefined")
            {
                dependencies.SearchFor[step.fromStep] = new Array(0);                   
                dependencies.Count++;
            }

        });

        return dependencies;            
    }

}

class Startup {
    public static main(): number {

        var loader = new Loader();
        var dependencies = loader.Load();

        var answer = "";
        while (dependencies.Count > 0)
        {
            var possible:string[] = new Array(0);
            Object.keys(dependencies.SearchFor).forEach(d => {
                var dependsOn = dependencies.SearchFor[d];
                if (dependsOn.length == 0) {
                    possible.push(d);
                }
            });
            var next = possible.sort((one, two) => (one > two ? 1 : -1))[0];

            Object.keys(dependencies.SearchFor).forEach(d => {
                var dependsOn = dependencies.SearchFor[d];
                
                var index: number = dependsOn.indexOf(next, 0);
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
    }
}

Startup.main();