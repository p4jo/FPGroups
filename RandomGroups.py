import networkx as nx
import random
import math

def assign_random_orientation(graph: nx.Graph): # also works with MultiGraph 
    for edge in graph.edges(data=True):
        this_vertex, other_end, data = edge
        data['start'] = random.choice([this_vertex, other_end])
    
def invert_generator(generator):
    return generator.upper() if not str.isupper(generator) else generator.lower()

a = ord('a')
def get_generator_names(K: int):
    return [ chr( a + i ) for i in range(K) ]

def assign_random_generators(graph: nx.Graph, generator_names):
    S = set(generator_names)
    SuSinv =  S.union( set( [ s.upper() for s in S ] ) )
    def randomGenerator(blocked_generators):
        return random.choice( list( SuSinv.difference(blocked_generators) ) )
    def assigned_outgoing_generators_at_vertex(vertex): 
        return set( [
            data['generator'] if data['start'] == vertex else invert_generator(data['generator'])
            for *_, data in graph.edges(vertex, data=True) 
            if 'generator' in data] 
        )
    
    for i in range(RETRIES := 10):
        for vertex in graph.nodes:
            assigned_generators = assigned_outgoing_generators_at_vertex(vertex)
            
            for edge in graph.edges(vertex, data=True):
                origin, target, data = edge
                # context: this_vertex, other_end, key, data = edge (omit key, needs actual MultiGraph)
                if 'generator' in data:
                    continue
                
                assigned_generators_at_target = [ invert_generator(g) for g in assigned_outgoing_generators_at_vertex(target) ]
                blocked_generators = assigned_generators.union(assigned_generators_at_target) if i < RETRIES - 1 else assigned_generators
                if len(blocked_generators) == len(SuSinv):
                    print("Warning: All generators are blocked at edge", edge)
                    break # break two loops (clever try/else XD)

                random_generator = randomGenerator(blocked_generators)
                
                assigned_generators.add(random_generator)
                data['generator'] = random_generator
            else: # all went well
                continue
            break # an edge was not assigned a generator
        else: # all went well
            break
        if i == RETRIES - 2:
            print("Warning: Could not assign all generators, will now ignore remote blocked generators")
        continue # at a vertex an edge was not assigned a generator
    else:
        print("Warning: Could not assign all generators") # should not happen. In the last retry, we don't look at remote blocked generators

def relators_from_graph(dir_graph: nx.Graph):# -> list[str]

    loops = list(nx.simple_cycles(dir_graph)) # Zykel, die keinen Vertex mehr als einmal besuchen. Sonst könnte man einen Subzykel nehmen

    relators = []
    for node_cycle in loops:
        l=len(node_cycle)
        if l == 2:
            continue # for non-multi-graphs, this gives the trivial relations aA
        
        edge_data_cycle = [
            dir_graph[ node_cycle[i] ][ node_cycle[(i + 1) % l] ]
            for i in range(l)
        ]
        generator_cycle_with_orientation = [
            edge_data['generator'] 
            if edge_data['start'] == node_cycle[i] 
            else invert_generator(edge_data['generator'])
            for i, edge_data in enumerate(edge_data_cycle)
        ]
        relators.append(''.join(generator_cycle_with_orientation))
    return relators


def random_group(valency = [6,5,5,4,3], proportion_of_generators = 0.75):
    graph = nx.expected_degree_graph(valency, selfloops=False) # selfloops: s.u.
    degree = max([d for n,d in graph.degree]) if not isinstance(graph.degree, int) else graph.degree
    # can be more than max(valency), bc of randomness

    assign_random_orientation(graph)
    # multi_graph = nx.MultiGraph( graph ) # s. Bem.

    K = math.ceil(degree * proportion_of_generators ) # number of generators
    generator_names = get_generator_names(K)
    assign_random_generators(graph, generator_names)


    relators = relators_from_graph(graph)
    return (generator_names, relators)