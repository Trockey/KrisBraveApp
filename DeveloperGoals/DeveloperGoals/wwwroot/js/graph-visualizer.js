// Graph Visualizer using vis.js
// Stores active graph instances
const graphInstances = new Map();

/**
 * Inicjalizuje graf dla danego kontenera
 * @param {string} containerId - ID kontenera HTML
 * @param {Array} nodes - Tablica węzłów grafu
 * @param {Array} edges - Tablica krawędzi grafu
 * @param {object} dotnetRef - Referencja do komponentu Blazor
 */
export function initializeGraph(containerId, nodes, edges, dotnetRef) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.error(`Container ${containerId} not found`);
        return;
    }

    // Przygotuj dane dla vis.js
    const visNodes = new vis.DataSet(nodes.map(node => ({
        id: node.id,
        label: node.name,
        title: createNodeTooltip(node),
        group: node.tag,
        level: node.isStart ? 0 : undefined,
        color: getNodeColor(node),
        font: {
            color: '#ffffff',
            size: 14,
            face: 'Inter, system-ui, sans-serif'
        },
        shape: node.isStart ? 'star' : 'dot',
        size: node.isStart ? 30 : 20,
        borderWidth: 2,
        borderWidthSelected: 4
    })));

    const visEdges = new vis.DataSet(edges.map(edge => ({
        id: edge.id,
        from: edge.from,
        to: edge.to,
        arrows: 'to',
        color: {
            color: '#64748b',
            highlight: '#3b82f6',
            hover: '#3b82f6'
        },
        width: 2,
        smooth: {
            type: 'cubicBezier',
            forceDirection: 'vertical',
            roundness: 0.4
        }
    })));

    // Opcje wizualizacji
    const options = {
        layout: {
            hierarchical: {
                enabled: true,
                direction: 'UD', // Up-Down
                sortMethod: 'directed',
                levelSeparation: 150,
                nodeSpacing: 200,
                treeSpacing: 250
            }
        },
        physics: {
            enabled: false // Wyłącz fizykę dla hierarchicznego layoutu
        },
        interaction: {
            hover: true,
            tooltipDelay: 200,
            zoomView: true,
            dragView: true,
            navigationButtons: true,
            keyboard: {
                enabled: true
            }
        },
        nodes: {
            shadow: {
                enabled: true,
                color: 'rgba(0,0,0,0.2)',
                size: 10,
                x: 2,
                y: 2
            }
        },
        edges: {
            shadow: {
                enabled: true,
                color: 'rgba(0,0,0,0.1)',
                size: 5,
                x: 1,
                y: 1
            }
        }
    };

    // Utwórz instancję grafu
    const network = new vis.Network(container, {
        nodes: visNodes,
        edges: visEdges
    }, options);

    // Event handlers
    network.on('click', function (params) {
        if (params.nodes.length > 0) {
            // Kliknięto węzeł
            const nodeId = params.nodes[0];
            dotnetRef.invokeMethodAsync('HandleNodeClick', nodeId);
        } else {
            // Kliknięto tło
            dotnetRef.invokeMethodAsync('HandleBackgroundClick');
        }
    });

    network.on('hoverNode', function () {
        container.style.cursor = 'pointer';
    });

    network.on('blurNode', function () {
        container.style.cursor = 'default';
    });

    // Zapisz instancję
    graphInstances.set(containerId, {
        network,
        nodes: visNodes,
        edges: visEdges,
        dotnetRef
    });

    // Wycentruj widok
    setTimeout(() => {
        network.fit({
            animation: {
                duration: 500,
                easingFunction: 'easeInOutQuad'
            }
        });
    }, 100);
}

/**
 * Aktualizuje graf nowymi danymi
 * @param {string} containerId - ID kontenera
 * @param {Array} nodes - Nowa tablica węzłów
 * @param {Array} edges - Nowa tablica krawędzi
 */
export function updateGraph(containerId, nodes, edges) {
    const instance = graphInstances.get(containerId);
    if (!instance) {
        console.error(`Graph instance ${containerId} not found`);
        return;
    }

    // Aktualizuj węzły
    const visNodes = nodes.map(node => ({
        id: node.id,
        label: node.name,
        title: createNodeTooltip(node),
        group: node.tag,
        level: node.isStart ? 0 : undefined,
        color: getNodeColor(node),
        font: {
            color: '#ffffff',
            size: 14,
            face: 'Inter, system-ui, sans-serif'
        },
        shape: node.isStart ? 'star' : 'dot',
        size: node.isStart ? 30 : 20,
        borderWidth: 2,
        borderWidthSelected: 4
    }));

    // Aktualizuj krawędzie
    const visEdges = edges.map(edge => ({
        id: edge.id,
        from: edge.from,
        to: edge.to,
        arrows: 'to',
        color: {
            color: '#64748b',
            highlight: '#3b82f6',
            hover: '#3b82f6'
        },
        width: 2,
        smooth: {
            type: 'cubicBezier',
            forceDirection: 'vertical',
            roundness: 0.4
        }
    }));

    // Zaktualizuj DataSets
    instance.nodes.clear();
    instance.nodes.add(visNodes);
    instance.edges.clear();
    instance.edges.add(visEdges);

    // Animacja dopasowania widoku
    setTimeout(() => {
        instance.network.fit({
            animation: {
                duration: 500,
                easingFunction: 'easeInOutQuad'
            }
        });
    }, 100);
}

/**
 * Niszczy instancję grafu
 * @param {string} containerId - ID kontenera
 */
export function destroyGraph(containerId) {
    const instance = graphInstances.get(containerId);
    if (instance) {
        instance.network.destroy();
        graphInstances.delete(containerId);
    }
}

/**
 * Tworzy tooltip dla węzła
 * @param {object} node - Obiekt węzła
 * @returns {string} HTML tooltip
 */
function createNodeTooltip(node) {
    const progressBar = `<div style="background: #e5e7eb; height: 8px; border-radius: 4px; margin-top: 8px;">
        <div style="background: ${getProgressColor(node.progress)}; height: 100%; width: ${node.progress}%; border-radius: 4px;"></div>
    </div>`;

    return `<div style="font-family: Inter, system-ui, sans-serif; padding: 4px;">
        <strong>${node.name}</strong><br/>
        <span style="color: #6b7280; font-size: 12px;">${node.prefix} • ${node.tag}</span><br/>
        <span style="font-size: 12px;">Postęp: ${node.progress}%</span>
        ${progressBar}
        ${node.systemDescription ? `<p style="margin-top: 8px; font-size: 12px; color: #4b5563;">${node.systemDescription}</p>` : ''}
    </div>`;
}

/**
 * Zwraca kolor węzła na podstawie postępu
 * @param {object} node - Obiekt węzła
 * @returns {object} Obiekt kolorów vis.js
 */
function getNodeColor(node) {
    if (node.isStart) {
        return {
            background: '#f59e0b',
            border: '#d97706',
            highlight: {
                background: '#fbbf24',
                border: '#f59e0b'
            }
        };
    }

    const progress = node.progress;
    let backgroundColor, borderColor;

    if (progress === 0) {
        // Nierozpoczęte - szary
        backgroundColor = '#9ca3af';
        borderColor = '#6b7280';
    } else if (progress < 30) {
        // Początkujący - czerwony
        backgroundColor = '#ef4444';
        borderColor = '#dc2626';
    } else if (progress < 70) {
        // Średniozaawansowany - żółty/pomarańczowy
        backgroundColor = '#f59e0b';
        borderColor = '#d97706';
    } else if (progress < 100) {
        // Zaawansowany - niebieski
        backgroundColor = '#3b82f6';
        borderColor = '#2563eb';
    } else {
        // Ukończone - zielony
        backgroundColor = '#10b981';
        borderColor = '#059669';
    }

    return {
        background: backgroundColor,
        border: borderColor,
        highlight: {
            background: backgroundColor,
            border: borderColor
        }
    };
}

/**
 * Zwraca kolor paska postępu
 * @param {number} progress - Postęp w procentach
 * @returns {string} Kolor hex
 */
function getProgressColor(progress) {
    if (progress === 0) return '#9ca3af';
    if (progress < 30) return '#ef4444';
    if (progress < 70) return '#f59e0b';
    if (progress < 100) return '#3b82f6';
    return '#10b981';
}
