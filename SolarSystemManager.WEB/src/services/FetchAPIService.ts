export default class FetchAPIService {
  private static baseURL: string = 'https://localhost:7247'

  static async get(url: string): Promise<any> {
    let response
    try {
      response = await fetch(this.baseURL + url, { mode: 'cors' })
      if (!response.ok) {
        throw new Error('Network response was not ok.')
      }
      console.log('FetchAPIService: ', response)
    } catch (error) {
      console.error('Network Error in FetchAPI Service: ', error)
      throw error
    }

    try {
      if (response) {
        const data = await response.text()
        return data
      }
    } catch (error) {
      throw new Error('Error parsing JSON')
    }
  }
}
