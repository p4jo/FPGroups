from sympy.combinatorics.free_groups import free_group, FreeGroupElement # wirft Error, wenn man import * macht !?!
from sympy.combinatorics.fp_groups import FpGroup # wirft Error, wenn man import * macht !?!
from sympy import Symbol
import re
from functools import reduce

a = ord('a')
def init_free_group(generator_names):
    global F, generators
    F, *generators = free_group(generator_names)

def textToSympySymbol(x):
    assert str.isalpha(x) and len(x) == 1, "x must be a single letter"
    if str.isupper(x):
        return textToSympySymbol(x.lower())**-1
    return generators[ord(x) - a]

def toSympyRelation(relator):
    return reduce(lambda a, b: a * b, [textToSympySymbol(x) for x in relator], F.identity)

def print_group(generator_names, relators: list[str]):
    unneccessary_generators = [ g for g in generator_names if g in relators or g.upper() in relators]
    
    pattern = "[" + "".join(unneccessary_generators) + "]"
    def remove_unnecessary_letters(r):
        return re.sub(pattern, "", r, flags=re.IGNORECASE)
    reduced_relators = [ remove_unnecessary_letters(r) for r in relators ] if unneccessary_generators else relators
    print('<', ', '.join(set(generator_names).difference(unneccessary_generators)), ' | ', ', '.join(r for r in reduced_relators if len(r) >= 1), '>', sep='')

def cyclically_reduce_relation(relation: FreeGroupElement):
    return relation.cyclic_reduction() 
# idea: if a generator appears ex. once, later replace all occurences and reduce number of generators

def reducedRelations(relations, reversed = False):
    res = []
    if reversed:
        relations.reverse()
    for r in relations:
        G = FpGroup(F, res)
        r_red: FreeGroupElement = G.reduce(r)
        if not r_red.is_identity:
            res.append( r_red )
            
    if reversed:
        res.reverse()
    return res

def sympySymbolToText(x: Symbol, pow = 1):
    if pow == 0:
        return ''
    if pow == 1:
        return x.name
    if pow > 1:
        # return x.name + '^' + str(pow)
        return x.name * pow
    return sympySymbolToText(x, -pow).upper()

def sympyRelationToText(relation: FreeGroupElement):
    return ''.join( sympySymbolToText(s, pow) for s, pow in relation.array_form )

from time import time
T = time()
def print_time():
    global T
    t = time()
    print(round(t - T, 2), end=': ')
    T = t


def reduce_relations(generator_names, relators) -> tuple[list[str], list[str]]:
    ## reduce relations. First time takes a long time
    init_free_group(generator_names)
    relations = [toSympyRelation(relator) for relator in relators]
    red_relations = relations
    for i in range(3):
        red_relations = reducedRelations(red_relations, reversed = i % 2 == 1)
        print_time()
    return (generator_names, [ sympyRelationToText(r) for r in red_relations])
