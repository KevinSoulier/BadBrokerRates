<template>
    <div>
        <h1 class="display-4 m-5">Bad Broker Rates</h1>
        <hr>
        <div class="row col-md-4 col-lg-4 offset-lg-4 offset-md-4">
            <div class="card">
                <div class="m-2">
                    <div class="">
                        <label for="startDate">Start</label>
                        <input id="startDate" v-model="startDate" class="form-control" type="date" />
                    </div>
                    <div class=" mt-2">
                        <label for="endDate">End</label>
                        <input id="endDate" v-model="endDate" class="form-control" type="date" />
                    </div>
                    <div class="mt-2">
                        <label for="Amount">Amount of Dollars</label>
                        <input id="Amount" v-model="amount" class="form-control" type="number" />
                    </div>
                    <div class="d-flex justify-content-left">
                        <button class="btn btn-primary align-self-center mt-2" v-on:click="SearchBestRate()">Search</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-5 mb-5">
            <div class="alert alert-danger col-lg-6 offset-lg-3" role="alert" v-if="errorMessage !== ''">
                {{errorMessage}}
            </div>
            <div class="card col-lg-6 offset-lg-3" v-if="showResult">
                <h4 class="mt-2" >Results</h4>
                <table id="SearchResult" class="col-lg-4">
                    <tr>
                        <td>Revenue</td>
                        <td>{{resultData.revenue}} USD</td>
                    </tr>
                    <tr>
                        <td>Instrument</td>
                        <td>{{resultData.instrument}}</td>
                    </tr>
                    <tr>
                        <td>Buy Date</td>
                        <td>{{resultData.buyDate}}</td>
                    </tr>
                    <tr>
                        <td>Sell Date</td>
                        <td>{{resultData.sellDate}}</td>
                    </tr>
                </table>
                <hr />

                <table class="table">
                    <thead>
                        <tr>
                            <th scope="col">Date</th>
                            <th scope="col">EUR</th>
                            <th scope="col">RUB</th>
                            <th scope="col">GBP</th>
                            <th scope="col">JPY</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(item, index) in this.resultData.currencyRates" v-bind:key="index">
                            <th> {{item.date}}</th>
                            <th> {{item.eur}}</th>
                            <th> {{item.rub}}</th>
                            <th> {{item.gbp}}</th>
                            <th> {{item.jpy}}</th>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";
    export default {
        name: 'BestRates',
        data() {
            return {
                startDate: (new Date()).toISOString().substr(0, 10),
                endDate: (new Date()).toISOString().substr(0, 10),
                amount: 0,
                showResult: false,
                resultData: {
                    revenue: 0,
                    instrument: "USD",
                    buyDate: (new Date()).toISOString().substr(0, 10),
                    sellDate: (new Date()).toISOString().substr(0, 10),
                    currencyRates: []
                },
                errorMessage: ""
            }
        },
        methods: {
            Init() {
                var dt = new Date();
                dt.setDate(dt.getDate() - 1);
                this.startDate = dt.toISOString().substr(0, 10);
            },
            SearchBestRate() {
                axios.get("/api/rates", { params: { startDate: this.startDate, endDate: this.endDate, moneyUsd: this.amount } })
                    .then((response) => {
                        console.log(response);
                        this.errorMessage = "";
                        this.resultData.revenue = response.data.revenue;
                        this.resultData.instrument = response.data.tool;
                        this.resultData.buyDate = response.data.buyDate.substr(0, 10);
                        this.resultData.sellDate = response.data.sellDate.substr(0, 10);
                        this.resultData.currencyRates = response.data.rates;

                        this.resultData.currencyRates.forEach((value) => {
                            value.date = value.date.substr(0, 10);
                        });
                        this.showResult = true;

                    }).catch(error => {
                        this.showResult = false;
                        this.errorMessage = error.response.data;
                    });
            }
            
        },
        created: function () {
            this.Init();
        }
    }
</script>

<style lang="scss" scoped>

</style>