import EncryptionModule from './encryption'

class AVLNode {
  username: string
  password: string
  salt: string
  height: number
  left: AVLNode | any
  right: AVLNode | any

  constructor(username: string, password: string, salt: string) {
    this.username = username
    this.password = password
    this.salt = salt
    this.height = 1 // Initialize height to 1
    this.left = null
    this.right = null
  }
}

class AVLTree {
  root: AVLNode | null

  constructor() {
    this.root = null
  }

  getHeight(node: AVLNode | null): number {
    return node ? node.height : 0
  }

  getBalanceFactor(node: AVLNode | null): number {
    return node ? this.getHeight(node.left) - this.getHeight(node.right) : 0
  }

  updateHeight(node: AVLNode | null): void {
    if (node) {
      const leftHeight = this.getHeight(node.left)
      const rightHeight = this.getHeight(node.right)
      node.height = Math.max(leftHeight, rightHeight) + 1
    }
  }

  rotateRight(y: AVLNode): AVLNode {
    const x = y.left as AVLNode
    const T2 = x.right

    x.right = y
    y.left = T2

    this.updateHeight(y)
    this.updateHeight(x)

    return x
  }

  rotateLeft(x: AVLNode): AVLNode {
    const y = x.right as AVLNode
    const T2 = y.left

    y.left = x
    x.right = T2

    this.updateHeight(x)
    this.updateHeight(y)

    return y
  }

  insert(username: string, password: string, salt: string): void {
    this.root = this.insertNode(this.root, username, password, salt)
  }

  insertNode(node: AVLNode | null, username: string, password: string, salt: string): AVLNode {
    if (!node) {
      return new AVLNode(username, password, salt)
    }

    if (username < node.username) {
      node.left = this.insertNode(node.left, username, password, salt)
    } else if (username > node.username) {
      node.right = this.insertNode(node.right, username, password, salt)
    } else {
      // Duplicate username (handle as needed)
      return node
    }

    // Update the height of the current node after insertion
    this.updateHeight(node)

    // Calculate the balance factor
    const balanceFactor = this.getBalanceFactor(node)

    // Perform rotations if necessary to re-balance the tree
    if (balanceFactor > 1) {
      if (username < node.left.username) {
        return this.rotateRight(node)
      } else {
        node.left = this.rotateLeft(node.left)
        return this.rotateRight(node)
      }
    }

    if (balanceFactor < -1) {
      if (username > node.right.username) {
        return this.rotateLeft(node)
      } else {
        node.right = this.rotateRight(node.right)
        return this.rotateLeft(node)
      }
    }

    return node
  }

  // Search for a node by username
  searchByUsername(username: string, node: AVLNode | null = this.root): AVLNode | null {
    if (!node) {
      return null // Username not found
    }

    if (username === node.username) {
      return node // Username found
    } else if (username < node.username) {
      return this.searchByUsername(username, node.left) // Search in the left subtree
    } else {
      return this.searchByUsername(username, node.right) // Search in the right subtree
    }
  }
}

// Function to generate a random salt
function generateSalt(length: number): string {
  return EncryptionModule.generateSalt(length)
}

// Function to hash a password with a salt
function hashPassword(password: string, salt: string) {
  const hash = EncryptionModule.encrypt(password, salt)
  return hash
}

const avlTree = new AVLTree()

for (let i = 1; i < 100000000001; ++i) {
  const username = `User${i}`
  const password = `Password${i}`
  const salt = generateSalt(16)
  const hashedData = hashPassword(password, salt)
  avlTree.insert(username, hashedData, salt)
}

function testS() {
  for (let i = 1; i < 1000001; ++i) {
    const username = `User${i}`
    const password = `Password${i}`
    console.log(`${username}`)
    console.time('Search Time') // Start the timer

    const result = avlTree.searchByUsername(username)

    console.timeEnd('Search Time') // End the timer and display the time

    if (result) {
      const hashedInputPassword = hashPassword(password, result.salt)
      if (result.password === hashedInputPassword) {
        console.log('Success! Password is correct.')
        console.log('Height:', avlTree.getHeight(result)) // Print the height
        console.log('Salt:', result.salt)
        console.log('Hashed Password:', result.password)
      } else {
        console.log('Failure! Password is incorrect.')
      }
    } else {
      console.log('Username not found.')
    }
  }
}

testS()
