import networkx as nx
import matplotlib.pyplot as plt

# Step 1: Create an empty directed graph
G = nx.DiGraph()

# Step 2: Add nodes to the graph for the first 500 integers
G.add_nodes_from(range(500))

# Step 3: For each integer, add edges to the graph corresponding to the generating set
generating_set = {1,2,4,8,16,32,64,128,256}
for i in range(500):
    for j in generating_set:
        if i+j < 500:
            G.add_edge(i, i+j)

#find the distance of n to 0 in G
def distance(G, n):
    return nx.shortest_path_length(G, 0, n)


# Step 4: Display the graph using matplotlib
plt.figure(figsize=(12,12))
nx.draw(G, with_labels=True)
plt.show()